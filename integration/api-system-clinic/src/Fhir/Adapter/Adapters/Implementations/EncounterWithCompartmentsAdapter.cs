// <copyright file="EncounterWithCompartmentsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Extensions;
using Hl7.Fhir.Rest;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Adapters.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IEncounterWithCompartmentsAdapter"/>
    /// </summary>
    internal class EncounterWithCompartmentsAdapter : IEncounterWithCompartmentsAdapter
    {
        /// <summary>
        /// Compartments which can be retrieved with the main resource.
        /// </summary>
        private static readonly string[] EncounterWithCompartmentsIncludes = { "Encounter:subject:Patient", "Encounter:individual:Practitioner" };

        /// <summary>
        /// Compartments which can be retrieved with the main resource.
        /// </summary>
        private static readonly string[] EncounterWithParticipantsIncludes = { "Encounter:individual:Practitioner" };

        /// <summary>
        /// Compartments which can be retrieved with the main resource.
        /// </summary>
        private static readonly string[] EncounterWithAppointmentIncludes = { "Encounter:appointment:Appointment", "Encounter:individual:Practitioner" };

        private static readonly string[] PatientList =
        {
            "eug3DevbSDb7srIov6WmLuQ3",
            "eyzb.Q2Foqv0g3YC9o.i6Ow3",
            "e1I578SHcm0OxOL-zc01FIg3",
            "eY92jvqYqkyV.sd1dc1bZcg3",
            "eSmZAxTF-fh-47g6ncG11EQ3",
            "eabVruTuRp2R4HrNMbO5cmw3",
            "ehmmzl8Ubj39aOb7rqRP7IQ3"
        };

        private readonly IFhirClientR4 _clientR4;
        private readonly IFhirClientSTU3 _clientStu3;
        private readonly IEncounterWithCompartmentsBuilder _builder;
        private readonly DateTime _desiredDate;

        public EncounterWithCompartmentsAdapter(
            IFhirClientR4 clientR4R4,
            IFhirClientSTU3 clientStu3,
            IEncounterWithCompartmentsBuilder builder,
            IConfiguration configuration)
        {
            _clientR4 = clientR4R4;
            _clientStu3 = clientStu3;
            _builder = builder;
            _desiredDate = configuration.GetValue<DateTime>("TmpEncountersDesiredDate");
        }

        public async Task<IEnumerable<EncounterWithCompartments>> FindEncountersWithCompartmentsAsync(SearchCriteria criteria)
        {
            //TODO: remove cycle when private API is available. This is a temp fix!
            var result = new List<EncounterWithCompartments>();
            foreach (var patient in PatientList)
            {
                var tmpCriteria = TempFixUntilWeDoNotHavePrivateApiForSearchCriteria(patient);

                var searchParams = ToSearchParams(tmpCriteria).AddIncludes(EncounterWithCompartmentsIncludes);
                var searchResult = await _clientR4.FindEncountersAsync(searchParams);

                var encounters = await _builder.ExtractEncountersWithCompartmentsAsync(searchResult, criteria.StartDateTime.GetValueOrDefault());

                //Practitioners are required in our scope, TODO: tbd.
                result.AddRange(encounters.Where(compartments => compartments.Practitioners.Length != 0));
            }

            return result;
        }

        public async Task<EncounterWithParticipants> FindEncounterWithParticipantsAsync(string encounterId)
        {
            var searchParams = new SearchParams().ById(encounterId).AddIncludes(EncounterWithParticipantsIncludes);
            var searchResult = await _clientR4.FindEncountersAsync(searchParams);

            return await _builder.ExtractEncounterWithParticipantsAsync(searchResult, true);
        }

        public async Task<IEnumerable<EncounterWithAppointment>> FindEncountersWithAppointmentsAsync(EncounterSearchCriteria criteria)
        {
            var searchParams = criteria.ToSearchParams().AddIncludes(EncounterWithAppointmentIncludes);
            var searchResult = await _clientStu3.FindEncountersAsync(searchParams);
            return await _builder.ExtractEncountersWithAppointmentsAsync(searchResult);
        }

        private static SearchParams ToSearchParams(SearchCriteria criteria)
        {
            var searchParams = new SearchParams();

            if (criteria.PatientId != null)
            {
                searchParams.ByPatientId(criteria.PatientId);
            }

            if (criteria.SiteId != null)
            {
                searchParams.BySiteId(criteria.SiteId);
            }

            if (criteria.StartDateTime != null)
            {
                searchParams.ByGreaterOrEqualDate((DateTime)criteria.StartDateTime);
                searchParams.ByLessOrEqualDate(criteria.StartDateTime.Value.AddDays(1));
            }

            return searchParams;
        }

        //TODO: rework when private api is available
        private SearchCriteria TempFixUntilWeDoNotHavePrivateApiForSearchCriteria(string patientId)
        {
            var tmpCriteria = new SearchCriteria
            {
                PatientId = patientId,
                StartDateTime = _desiredDate
            };
            return tmpCriteria;
        }
    }
}
