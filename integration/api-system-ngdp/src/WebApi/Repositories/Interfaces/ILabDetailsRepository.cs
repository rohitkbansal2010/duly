// <copyright file="ILabDetailsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="LabDetails"/> entities.
    /// </summary>
    public interface ILabDetailsRepository
    {
        /// <summary>
        /// Save all items of <see cref="LabDetails"/> which match with the filter.
        /// </summary>
        /// <param name="request">Identifier of clinic.</param>
        /// <returns>Filtered items of <see cref="LabDetails"/>.</returns>
        Task<int> PostLabDetailsAsync(LabDetails request);


        /// <summary>
        /// Retrieve <see cref="LabLocation"/> which match with the referral id.
        /// </summary>
        /// <param name="lat">Latitude.</param>
        /// <param name="lng">Longitude.</param>
        /// <returns>Item of <see cref="LabLocation"/>.</returns>
        public Task<IEnumerable<LabLocation>> GetLabLocationByLatLng(string lat, string lng);
    }
}