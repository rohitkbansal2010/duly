// <copyright file="IProviderRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Models.Provider"/> model.
    /// </summary>
    public interface IProviderRepository
    {
        public Task<IEnumerable<Models.Provider>> GetProvidersByLatLngAsync(string lat, string lng, string providerType);

        /// <summary>
        /// Returns <see cref="Models.CheckOut.ProviderDetails"/> for a specific provider.
        /// </summary>
        /// <param name="providerIds">Identifier of the provider.</param>
        /// <returns>A <see cref="Models.CheckOut.ProviderDetails"/> instance.</returns>
        Task<IEnumerable<Models.CheckOut.ProviderDetails>> GetProviderDetailsAsync(string providerIds);
    }
}
