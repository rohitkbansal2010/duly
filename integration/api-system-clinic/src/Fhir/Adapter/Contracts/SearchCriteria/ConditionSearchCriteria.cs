// <copyright file="ConditionSearchCriteria.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using Duly.Clinic.Fhir.Adapter.Exceptions;
using Duly.Clinic.Fhir.Adapter.Extensions;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Utility;
using System.Linq;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria
{
    /// <summary>
    /// Search criteria for Conditions.
    /// </summary>
    public class ConditionSearchCriteria : SearchCriteria
    {
        /// <summary>
        /// Category of Condition e.g. problem-list-item, genomics.
        /// </summary>
        public string[] Categories { get; set; }

        /// <summary>
        /// Clinical statuses of condition.
        /// </summary>
        public R4.Condition.ConditionClinicalStatusCodes[] Statuses { get; set; }

        internal SearchParams ToSearchParams()
        {
            var searchParams = new SearchParams();

            if (string.IsNullOrEmpty(PatientId))
            {
                throw new MandatoryQueryParameterMissingException("Patient Id is missing.");
            }

            searchParams.ByPatientId(PatientId);

            if (Categories == null || Categories.Length == 0)
            {
                throw new MandatoryQueryParameterMissingException("Condition Category is missing.");
            }

            //TODO: Remove this check once version is changed to August 2021 or later.
            if (Categories.Length > 1)
            {
                throw new ContradictoryQueryException("Current version of EPIC supports only one category at a time.");
            }

            foreach (var category in Categories)
            {
                searchParams.ByCategory(category);
            }

            if (Statuses != null && Statuses.Length != 0)
            {
                var concatenated = string.Join(SearchParamsExtensions.EntityKeysSeparator, Statuses.Select(x => x.GetLiteral()));
                searchParams.ByClinicalStatus(concatenated);
            }

            return searchParams;
        }
    }
}
