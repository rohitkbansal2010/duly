// <copyright file="EncounterConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;
using AutoMapper;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts;
using System.Collections.Generic;
using System.Linq;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps http://hl7.org/fhir/StructureDefinition/Encounter
    /// with
    ///     http://hl7.org/fhir/StructureDefinition/Patient with amount of past visits
    ///     and <see cref="PractitionerWithRoles"/>
    /// into <see cref="Contracts.Encounter"/>.
    /// </summary>
    public class EncounterConverter : ITypeConverter<EncounterWithCompartments, Encounter>
    {
        public Encounter Convert(EncounterWithCompartments source, Encounter destination, ResolutionContext context)
        {
            var encounter = new Encounter
            {
                Id = source.Resource.Id,
                ServiceType = ConvertServiceType(source.Resource.ServiceType),
                Type = ConvertClassToType(source.Resource.Class),
                Status = ConvertStatus(source.Resource.Status, context.Mapper),
                TimeSlot = ConvertTimeSlot(source.Resource.Period, context.Mapper),
                Location = ConvertLocation(source.Resource.Location, context.Mapper),
                Patient = ConvertPatientGeneralInfoWithVisitsHistory(source.Patient, source.DoesPatientHavePastVisits, context.Mapper),
                Practitioner = ConvertPractitioner(source, context)
            };

            return encounter;
        }

        private static PractitionerGeneralInfo ConvertPractitioner(EncounterWithCompartments source, ResolutionContext context)
        {
            //TODO: identify one of the Practitioners
            return context.Mapper.Map<PractitionerGeneralInfo>(source.Practitioners.FirstOrDefault());
        }

        private static string ConvertServiceType(Hl7.Fhir.Model.CodeableConcept sourceServiceType)
        {
            //TODO: redesign
            return sourceServiceType?.Text ?? "n/a";
        }

        private static EncounterType ConvertClassToType(Hl7.Fhir.Model.Coding classOfEncounter)
        {
            //TODO: define algorithm
            switch (classOfEncounter.Code)
            {
                case "IMP":
                    return EncounterType.OnSite;
                case "VR":
                    return EncounterType.Telehealth;
                default:
                    return EncounterType.OnSite;
            }
        }

        private static EncounterStatus ConvertStatus(
            R4.Encounter.EncounterStatus? encounterStatus,
            IRuntimeMapper contextMapper)
        {
            //TODO: redesign
            if (encounterStatus == R4.Encounter.EncounterStatus.Unknown)
                return EncounterStatus.Cancelled;

            return contextMapper.Map<EncounterStatus>(encounterStatus);
        }

        private static TimeSlot ConvertTimeSlot(
            Hl7.Fhir.Model.Period encounterPeriod,
            IRuntimeMapper contextMapper)
        {
            return contextMapper.Map<TimeSlot>(encounterPeriod);
        }

        private static Location ConvertLocation(
            IEnumerable<R4.Encounter.LocationComponent> encounterLocation,
            IRuntimeMapper contextMapper)
        {
            //TODO: identify the algorithm
            var location = encounterLocation.FirstOrDefault();
            return contextMapper.Map<Location>(location);
        }

        private static PatientGeneralInfoWithVisitsHistory ConvertPatientGeneralInfoWithVisitsHistory(
            R4.Patient sourcePatient,
            bool hasPastVisits,
            IRuntimeMapper contextMapper)
        {
            var patient = new PatientGeneralInfoWithVisitsHistory
            {
                HasPastVisits = hasPastVisits,
                Patient = contextMapper.Map<PatientGeneralInfo>(sourcePatient)
            };

            return patient;
        }
    }
}