// <copyright file="ICheckOutDetailsService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.CheckOutDetails"/> entity.
    /// </summary>
    public interface ICheckOutDetailsservice
    {
        /// <summary>
        /// Retrieve <see cref="Contracts.CheckOutDetails"/> that represents an information about LabDetails, Followup, Schedule, Imaging.
        /// </summary>
        /// <param name="appointmentId">The identifier of a specific patient.</param>
        /// <returns>An instance of <see cref="Contracts.CheckOutDetails"/> for a specific patient.</returns>
        Task<Contracts.CheckOutDetails> GetCheckOutDetailsAsync(string appointmentId);
    }
}
