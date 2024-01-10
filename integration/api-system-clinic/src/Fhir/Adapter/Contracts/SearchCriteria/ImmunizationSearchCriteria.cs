// <copyright file="ImmunizationSearchCriteria.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Exceptions;
using Duly.Clinic.Fhir.Adapter.Extensions;
using Hl7.Fhir.Rest;
using System.Linq;

namespace Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria
{
    /// <summary>
    /// Search criteria for Immunizations.
    /// </summary>
    public class ImmunizationSearchCriteria : SearchCriteria
    {
        /// <summary>
        /// Statuses of Immunizations e.g. completed, entered-in-error, not-done.
        /// </summary>
        public string[] Statuses { get; set; }

        internal SearchParams ToSearchParams()
        {
            if (string.IsNullOrEmpty(PatientId))
            {
                throw new MandatoryQueryParameterMissingException("Patient Id is missing.");
            }

            var searchParams = new SearchParams();

            searchParams.ByPatientId(PatientId);

            if (Statuses?.Any() ?? false)
            {
                searchParams.ByStatus(string.Join(SearchParamsExtensions.EntityKeysSeparator, Statuses.Select(c => c.ToLower())));
            }

            return searchParams;
        }
    }
}
