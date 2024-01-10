// <copyright file="ICheckOutDetailsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Models.CheckOut.CheckOutDetails"/> model.
    /// </summary>
    public interface ICheckOutDetailsRepository
    {
        /// <summary>
        /// Returns <see cref="Models.CheckOut.CheckOutDetails"/> for a specific patient.
        /// </summary>
        /// <param name="appointmentId">Identifier of the patient.</param>
        /// <returns>A <see cref="Models.CheckOut.CheckOutDetails"/> instance.</returns>
        Task<Models.CheckOut.CheckOutDetails> GetCheckOutDetailsByIdAsync(string appointmentId);
    }
}
