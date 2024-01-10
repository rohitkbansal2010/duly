// <copyright file="DiagnosticReportSearchCriteria.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Exceptions;
using Duly.Clinic.Fhir.Adapter.Extensions;
using Hl7.Fhir.Rest;

namespace Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria
{
    /// <summary>
    /// Search criteria for DiagnosticReport.
    /// </summary>
    public class DiagnosticReportSearchCriteria : SearchCriteria
    {
        /// <summary>
        /// Loinc codes of desired Diagnostic report Types.
        /// </summary>
        public string[] Codes { get; set; }

        internal SearchParams ToSearchParams()
        {
            var searchParams = new SearchParams();

            searchParams.ByPatientId(PatientId);

            if (Codes != null && Codes.Length != 0)
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
                    throw new ContradictoryQueryException("Diagnostic report query EndDateTime < StartDateTime");
                }

                searchParams.ByLessOrEqualDate((System.DateTime)EndDateTime);
            }

            return searchParams;
        }
    }
}