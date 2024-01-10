// <copyright file="IPractitionerWithCompartmentsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// Adapter to work on Practitioners with compartments.
    /// </summary>
    public interface IPractitionerWithCompartmentsAdapter
    {
        /// <summary>
        /// Gets Practitioners and roles associated with them.
        /// </summary>
        /// <param name="criteria">Search criteria. Should correctly work only with siteId. Other filters might return correct result, but it's tricky.</param>
        /// <returns>All data that satisfies search criteria.</returns>
        Task<IEnumerable<PractitionerWithRoles>> FindPractitionersWithRolesAsync(SearchCriteria criteria);

        /// <summary>
        /// Gets Practitioners and roles associated with them by identifiers.
        /// </summary>
        /// <param name="identifiers">Identifiers of the practitioner.</param>
        /// <returns>All data that satisfies defined identifiers.</returns>
        Task<IEnumerable<PractitionerWithRoles>> FindPractitionersByIdentifiersAsync(string[] identifiers);
    }
}