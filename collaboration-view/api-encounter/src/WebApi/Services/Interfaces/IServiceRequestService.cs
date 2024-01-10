// <copyright file="IServiceRequestService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.ServiceRequest"/> entity.
    /// </summary>
    public interface IServiceRequestService
    {
        /// <summary>
        /// Returns <see cref="Contracts.ServiceRequest"/> for a specific patient.
        /// </summary>
        /// <param name="patientId">Identifier of the patient.</param>
        /// <param name="appointmentId">appointment Id.</param>
        /// <param name="type">Imaging or labs.</param>
        /// <returns>A <see cref="Contracts.ServiceRequest"/> instance.</returns>
        Task<Contracts.ServiceRequest> GetLabOrImagingOrdersAsync(string patientId, string appointmentId, string type);
    }
}
