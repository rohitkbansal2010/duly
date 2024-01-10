// <copyright file="Immunization.SearchCriteria.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.Ngdp.Adapter.Adapters.Models
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contracts")]
    public class ImmunizationSearchCriteria
    {
        /// <summary>
        /// Identity of the patient.
        /// </summary>
        public string PatientId { get; init; }

        /// <summary>
        /// Due statuses which should be included by Id.
        /// </summary>
        public int[] IncludedDueStatusesIds { get; init; }

        public dynamic ConvertToParameters()
        {
            var parameters = new
            {
                PatientId,
                IncludedDueStatusIds = BuildIncludedDueStatusIds()
            };

            return parameters;
        }

        private string BuildIncludedDueStatusIds()
        {
            return IncludedDueStatusesIds == null
                ? string.Empty
                : string.Join(',', IncludedDueStatusesIds);
        }
    }
}