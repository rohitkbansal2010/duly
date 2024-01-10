// <copyright file="IServiceRequestRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Models.ServiceRequest"/> model.
    /// </summary>
    internal interface IServiceRequestRepository
    {
        /// <summary>
        /// Returns <see cref="Models.ServiceRequest"/> for a specific patient.
        /// </summary>
        /// <param name="patientId">Identifier of the patient.</param>
        /// <param name="appointmentId">appointment Id.</param>
        /// <param name="type">Labs or Imaginig.</param>
        /// <returns>A <see cref="Models.ServiceRequest"/> instance.</returns>
        Task<Models.ServiceRequest> GetLabsOrImagingAsync(string patientId,  string appointmentId, string type);
    }
}
