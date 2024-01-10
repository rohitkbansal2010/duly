// <copyright file="ILabDetailService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.LabDetail"/> entity.
    /// </summary>
    public interface ILabDetailService
    {
        /// <summary>
        /// Returns Lab Detail response.
        /// </summary>
        /// <param name="request"><see cref="Contracts.LabDetail"/>LabDetail.</param>
        /// <returns>Returns cvCheckOut_ID.</returns>
        Task<int> PostLabDetailAsync(LabDetail request);

        /// <summary>
        /// Returns Lab Location Details.
        /// </summary>
        /// <param name="lat"><see cref="Contracts.LabLocation"/>Latitude.</param>
        /// <param name="lng"><see cref="Contracts.LabLocation"/>Longitude.</param>
        /// <returns>Returns cvCheckOut_ID.</returns>
        public Task<IEnumerable<Contracts.LabLocation>> GetLabLocationByLatLngAsync(string lat, string lng);
    }
}
