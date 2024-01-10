// <copyright file="IPatientService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.Patient"/> entity.
    /// </summary>
    public interface IPatientService
    {
        /// <summary>
        /// Returns <see cref="Contracts.Patient"/> for a specific patient.
        /// </summary>
        /// <param name="patientId">Identifier of the patient.</param>
        /// <returns>A <see cref="Contracts.Patient"/> instance.</returns>
        Task<Contracts.Patient> GetPatientByIdAsync(string patientId);
    }
}