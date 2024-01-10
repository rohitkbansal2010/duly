// <copyright file="IImmunizationService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.Immunizations"/> entity.
    /// </summary>
    public interface IImmunizationService
    {
        /// <summary>
        /// Retrieve <see cref="Contracts.Immunizations"/> that represents an information about a patient's immunization,
        /// including information on recommended and past immunizations, as well as the patient's immunization progress.
        /// </summary>
        /// <param name="patientId">The identifier of a specific patient.</param>
        /// <returns>An instance of <see cref="Contracts.Immunizations"/> for a specific patient.</returns>
        Task<Contracts.Immunizations> GetImmunizationsByPatientIdAsync(string patientId);
    }
}
