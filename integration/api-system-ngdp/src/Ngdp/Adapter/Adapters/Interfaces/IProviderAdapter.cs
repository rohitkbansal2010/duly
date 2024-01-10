// <copyright file="IProviderAdapter.cs" >company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// An adapter for working with "ProviderLocation" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface IProviderAdapter
    {
        /// <summary>
        /// Finds matching items of <see cref="ProviderSearchCriteria"/> by <see cref="searchCriteria"/>.
        /// </summary>
        /// <param name="searchCriteria">Criteria to search by.</param>
        /// <returns>Filtered items of <see cref=""/>.</returns>
        public Task<IEnumerable<ProviderLocation>> FindProviderByLatLngAsync(ProviderSearchCriteria searchCriteria);

        /// <summary>
        /// Finds matching items of <see cref="ProviderDetails"/> by <see cref="providerId"/>.
        /// </summary>
        /// <param name="providerIds">Criteria to search by.</param>
        /// <returns>Filtered items of <see cref="ProviderDetails"/>.</returns>
        Task<IEnumerable<ProviderDetails>> FindProviderDetailsAsync(string providerIds);
    }
}
