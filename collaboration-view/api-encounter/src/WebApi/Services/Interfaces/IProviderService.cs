// <copyright file="IProviderService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.Provider"/> entity.
    /// </summary>
    public interface IProviderService
    {
        /// <summary>
        /// Returns Lab Provider response.
        /// </summary>
        /// <param name="lat"><see cref="Contracts.Provider"/>Latitude.</param>
        /// <param name="lng"><see cref="Contracts.Provider"/>Longitude.</param>
        /// <param name="providerType"><see cref="Contracts.Provider"/>providerType.</param>
        /// <returns>Returns cvCheckOut_ID.</returns>
        public Task<IEnumerable<Contracts.Provider>> GetProvidersByLatLngAsync(string lat, string lng, string providerType);

        /// <summary>
        /// Retrieve <see cref="Contracts.ProviderDetails"/> that represents an information about Provider.
        /// </summary>
        /// <param name="providerIds">The identifier of a specific provider.</param>
        /// <returns>An instance of <see cref="Contracts.ProviderDetails"/> for a specific provider.</returns>
        Task<IEnumerable<Contracts.ProviderDetails>> GetProviderDetailsAsync(string providerIds);
    }
}
