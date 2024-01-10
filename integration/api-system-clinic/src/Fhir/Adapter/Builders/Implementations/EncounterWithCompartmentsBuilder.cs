// <copyright file="EncounterWithCompartmentsBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;
extern alias stu3;

using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Extensions;
using Duly.Clinic.Fhir.Adapter.Extensions.Fhir;
using Hl7.Fhir.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;
using STU3 = stu3::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Builders.Implementations
{
    /// <summary>
    /// Builds, finds and puts together compartments for Encounter.
    /// </summary>
    internal class EncounterWithCompartmentsBuilder : IEncounterWithCompartmentsBuilder
    {
        private readonly IFhirClientR4 _client;
        private readonly IPractitionerWithRolesBuilder _practitionerWithRolesBuilder;

        public EncounterWithCompartmentsBuilder(
            IFhirClientR4 client,
            IPractitionerWithRolesBuilder practitionerWithRolesBuilder)
        {
            _client = client;
            _practitionerWithRolesBuilder = practitionerWithRolesBuilder;
        }

        public async Task<EncounterWithCompartments[]> ExtractEncountersWithCompartmentsAsync(IEnumerable<R4.Bundle.EntryComponent> searchResult, DateTime date)
        {
            // searchResult = encounters + practitioners + patients
            var searchResultArray = searchResult.ToArray();
            if (searchResultArray.Length == 0)
            {
                return Array.Empty<EncounterWithCompartments>();
            }

            var patients = searchResultArray.Select(x => x.Resource).OfType<R4.Patient>().ToArray();
            var encounters = searchResultArray.Select(component => component.Resource).OfType<R4.Encounter>().ToArray();

            var pastEncounters = await FindPastEncounters(patients, date);

            var practitionersWithRoles = await _practitionerWithRolesBuilder.RetrievePractitionerWithRolesAsync(searchResultArray);

            return encounters.Select(encounter => BuildFromSearchResults(encounter, patients, pastEncounters, practitionersWithRoles)).ToArray();
        }

        public async Task<EncounterWithParticipants> ExtractEncounterWithParticipantsAsync(
            IEnumerable<R4.Bundle.EntryComponent> searchResult,
            bool shouldLeaveActivePractitioners = false)
        {
            // searchResult = encounter + practitioners
            var searchResultArray = searchResult.ToArray();
            var practitionersWithRoles = await _practitionerWithRolesBuilder.RetrievePractitionerWithRolesAsync(searchResultArray, shouldLeaveActivePractitioners);
            var encounter = searchResultArray.Select(component => component.Resource).OfType<R4.Encounter>().Single();

            return new EncounterWithParticipants
            {
                Resource = encounter,
                Practitioners = practitionersWithRoles.ToArray()
            };
        }

        public async Task<EncounterWithAppointment[]> ExtractEncountersWithAppointmentsAsync(IEnumerable<STU3.Bundle.EntryComponent> searchResult)
        {
            // searchResult = encounters + appointments + practitioners
            var searchResultArray = searchResult.ToArray();
            if (searchResultArray.Length == 0)
            {
                return Array.Empty<EncounterWithAppointment>();
            }

            var encounters = searchResultArray.Select(component => component.Resource).OfType<STU3.Encounter>().ToArray();
            var appointments = searchResultArray.Select(component => component.Resource).OfType<STU3.Appointment>().ToArray();
            var practitionersWithRoles = await _practitionerWithRolesBuilder.RetrievePractitionerWithRolesAsync(searchResultArray);

            return encounters.Select(encounter => BuildFromSearchResults(encounter, appointments, practitionersWithRoles)).ToArray();
        }

        /// <summary>
        /// Builds one EncounterWithCompartments.
        /// </summary>
        /// <param name="encounter">Primary resource.</param>
        /// <param name="patients">Patients.</param>
        /// <param name="pastEncounters">Past patients encounters.</param>
        /// <param name="practitionersWithRoles">Practitioners with roles.</param>
        /// <returns>Encounter with compartments.</returns>
        private static EncounterWithCompartments BuildFromSearchResults(
            R4.Encounter encounter,
            IEnumerable<R4.Patient> patients,
            IEnumerable<R4.Encounter> pastEncounters,
            IEnumerable<PractitionerWithRoles> practitionersWithRoles)
        {
            var result = new EncounterWithCompartments
            {
                Resource = encounter,
                Patient = FindPatientsForEncounter(patients, encounter.Subject.Reference),
                DoesPatientHavePastVisits = AnyPastEncountersForPatient(pastEncounters, encounter.Subject.Reference),
                Practitioners = FindPractitionersForEncounter(practitionersWithRoles, encounter)
            };

            return result;
        }

        private static EncounterWithAppointment BuildFromSearchResults(
            STU3.Encounter encounter,
            IEnumerable<STU3.Appointment> appointments,
            IEnumerable<PractitionerWithRolesSTU3> practitionersWithRoles)
        {
            var result = new EncounterWithAppointment
            {
                Resource = encounter,
                Appointment = FindAppointmentForEncounter(appointments, encounter),
                Practitioners = FindPractitionersForEncounter(practitionersWithRoles, encounter)
            };

            return result;
        }

        private static R4.Patient FindPatientsForEncounter(IEnumerable<R4.Patient> patients, string patientResourceRef)
        {
            return patients.First(x => x.ToReference() == patientResourceRef);
        }

        private static bool AnyPastEncountersForPatient(IEnumerable<R4.Encounter> encounters, string patientResourceRef)
        {
            return encounters.Any(x => x.Subject.Reference == patientResourceRef);
        }

        private static PractitionerWithRoles[] FindPractitionersForEncounter(IEnumerable<PractitionerWithRoles> practitionersWithRoles, R4.Encounter encounter)
        {
            return practitionersWithRoles.Where(x =>
                encounter.Participant.Any(y => x.Resource.ToReference() == y.Individual.Reference)).ToArray();
        }

        private static PractitionerWithRolesSTU3[] FindPractitionersForEncounter(IEnumerable<PractitionerWithRolesSTU3> practitionersWithRoles, STU3.Encounter encounter)
        {
            return practitionersWithRoles.Where(x =>
                encounter.Participant.Any(y => x.Resource.ToReference() == y.Individual.Reference)).ToArray();
        }

        private static STU3.Appointment FindAppointmentForEncounter(IEnumerable<STU3.Appointment> appointments, STU3.Encounter encounter)
        {
            return encounter.Appointment == null
                ? null
                : appointments.FirstOrDefault(x => encounter.Appointment.Reference.Contains(x.ToReference()));
        }

        private async Task<R4.Encounter[]> FindPastEncounters(IEnumerable<R4.Patient> patients, DateTime date)
        {
            var searchParams = new SearchParams().BySubjectReferences(patients).ByLessDate(date);
            var pastEncounters = await _client.FindResourcesAsync<R4.Encounter>(searchParams);
            return pastEncounters.ToArray();
        }
    }
}
