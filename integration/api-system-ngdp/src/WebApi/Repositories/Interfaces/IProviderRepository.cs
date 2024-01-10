// <copyright file="IProviderRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="ProviderLocation"/> entities.
    /// </summary>
    public interface IProviderRepository
    {
        /// <summary>
        /// Retrieve <see cref="ProviderLocation"/> which match with the referral id.
        /// </summary>
        /// <param name="lat">Location Id.</param>
        /// <param name="lng">Filter by start date.</param>
        /// <param name="providerType">Filter by end date.</param>
        /// <returns>Item of <see cref="ProviderLocation"/>.</returns>
        public Task<IEnumerable<ProviderLocation>> GetProvidersByLatLng(string lat, string lng, string providerType);

        /// <summary>
        /// Retrieve all items of <see cref="ProviderDetails"/> which match with the filter.
        /// </summary>
        /// <param name="providerIds">Provider Id.</param>
        /// <returns>Filtered items of <see cref="ProviderDetails"/>.</returns>
        Task<IEnumerable<ProviderDetails>> GetProviderDetailsAsync(string providerIds);
    }
}
