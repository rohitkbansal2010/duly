// <copyright file="IImagingDetailService.cs" company="Duly Health and Care">
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
    /// the corresponding contract for the <see cref="Contracts.ImagingDetail"/> entity.
    /// </summary>
    public interface IImagingDetailService
    {
        /// <summary>
        /// Returns Imaging Detail response.
        /// </summary>
        /// <param name="request"><see cref="Contracts.ImagingDetail"/>ImagingDetail.</param>
        /// <returns>Returns cvCheckOut_ID.</returns>
        Task<int> PostImagingDetailAsync(ImagingDetail request);
    }
}