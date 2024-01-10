// <copyright file="IImmunizationAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// An adapter for working with "Immunizations" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface IImmunizationAdapter
    {
        /// <summary>
        /// Finds matching items of <see cref="Immunization"/> by <see cref="searchCriteria"/>.
        /// </summary>
        /// <param name="searchCriteria">Criteria to search by.</param>
        /// <returns>Filtered items of <see cref="Immunization"/>.</returns>
        Task<IEnumerable<Immunization>> FindImmunizationsAsync(ImmunizationSearchCriteria searchCriteria);
    }
}