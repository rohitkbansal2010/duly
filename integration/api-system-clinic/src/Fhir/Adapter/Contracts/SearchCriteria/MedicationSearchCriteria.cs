// <copyright file="MedicationSearchCriteria.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Extensions;
using Hl7.Fhir.Rest;
using System.Linq;

namespace Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria
{
    /// <summary>
    /// Search criteria for Medications.
    /// </summary>
    public class MedicationSearchCriteria : SearchCriteria
    {
        /// <summary>
        /// Categories of Medications e.g. inpatient, outpatient, community, patientspecified.
        /// </summary>
        public string[] Categories { get; set; }

        internal SearchParams ToSearchParams()
        {
            var searchParams = new SearchParams();

            if (PatientId != null)
            {
                searchParams.ByPatientId(PatientId);
            }

            if (Status != null)
            {
                searchParams.ByStatus(Status.ToLower());
            }

            if (Categories?.Any() ?? false)
            {
                var categories = string.Join(SearchParamsExtensions.EntityKeysSeparator, Categories.Select(c => c.ToLower()));
                searchParams.ByCategory(categories);
            }

            return searchParams;
        }
    }
}
