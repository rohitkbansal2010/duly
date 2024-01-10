// <copyright file="IImagingRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="ImagingDetail"/> entities.
    /// </summary>
    public interface IImagingRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="ImagingDetail"/> which match with the filter.
        /// </summary>
        /// <param name="request">Identifier of clinic.</param>
        /// <returns>Filtered items of <see cref="ImagingDetail"/>.</returns>
        Task<int> PostImagingAsync(ImagingDetail request);
    }
}