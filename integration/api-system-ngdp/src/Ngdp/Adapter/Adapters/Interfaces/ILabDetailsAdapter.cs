// <copyright file="ILabDetailsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// An adapter for working with "LabDetails" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface ILabDetailsAdapter
    {
        /// <summary>
        /// Finds matching items of <see cref="LabDetails"/>.
        /// </summary>
        /// <param name="request">Criteria to search by.</param>
        /// <returns>Filtered items of <see cref="LabDetails"/>.</returns>
        Task<int> PostLabDetailsAsync(LabDetails request);

        /// <summary>
        /// Finds matching items of <see cref="LabLocationSearchCriteria"/> by <see cref="searchCriteria"/>.
        /// </summary>
        /// <param name="searchCriteria">Criteria to search by.</param>
        /// <returns>Filtered items of <see cref=""/>.</returns>
        public Task<IEnumerable<LabLocation>> FindLabLocationByLatLngAsync(LabLocationSearchCriteria searchCriteria);
    }
}