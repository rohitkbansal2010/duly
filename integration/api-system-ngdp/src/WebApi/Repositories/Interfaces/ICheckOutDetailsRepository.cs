// <copyright file="ICheckOutDetailsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="CheckOutDetails"/> entities.
    /// </summary>
    public interface ICheckOutDetailsRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="CheckOutDetails"/> which match with the filter.
        /// </summary>
        /// <param name="appointmentId">Identifier of clinic.</param>
        /// <returns>Filtered items of <see cref="CheckOutDetails"/>.</returns>
        Task<CheckOutDetails> GetCheckOutDetailsAsync(string appointmentId);
    }
}