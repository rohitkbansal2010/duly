// <copyright file="IServiceRequestRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="ServiceRequest"/> entities.
    /// </summary>
    public interface IServiceRequestRepository
    {
        /// <summary>
        /// Retrieve a <see cref="ServiceRequest"/> with details by specific Id.
        /// </summary>
        /// <param name="patientId">Identifier of patient.</param>
        /// <param name="appointmentId">encounter appointment Id.</param>
        /// <param name="type">Imaging or labs.</param>
        /// <returns><see cref="ServiceRequest"/> with details.</returns>
        public Task<ServiceRequest> GetLabOrdersAsync(string patientId, string appointmentId, string type);
    }
}
