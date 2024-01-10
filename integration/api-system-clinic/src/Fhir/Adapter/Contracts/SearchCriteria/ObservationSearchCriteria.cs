// <copyright file="ObservationSearchCriteria.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Exceptions;
using Duly.Clinic.Fhir.Adapter.Extensions;
using Hl7.Fhir.Rest;

namespace Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria
{
    /// <summary>
    /// Search criteria for Observations.
    /// </summary>
    public class ObservationSearchCriteria : SearchCriteria
    {
        /// <summary>
        /// Category of Observations e.g. vital-signs, LDA, core-characteristics.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Loinc codes of desired observation Types.
        /// </summary>
        public string[] Codes { get; set; }

        internal SearchParams ToSearchParams()
        {
            var searchParams = new SearchParams();

            searchParams.ByPatientId(PatientId);

            if (string.IsNullOrEmpty(Category))
            {
                throw new MandatoryQueryParameterMissingException("Observation Category is missing.");
            }

            searchParams.ByCategory(Category);

            if (Codes is not null && Codes.Length is not 0)
            {
                searchParams.ByCodes(Codes);
            }

            if (StartDateTime.HasValue)
            {
                searchParams.ByGreaterOrEqualDate((System.DateTime)StartDateTime);
            }

            if (EndDateTime.HasValue)
            {
                if (EndDateTime < StartDateTime)
                {
                    throw new ContradictoryQueryException("Observation query EndDateTime < StartDateTime");
                }

                searchParams.ByLessOrEqualDate((System.DateTime)EndDateTime);
            }

            return searchParams;
        }
    }
}
