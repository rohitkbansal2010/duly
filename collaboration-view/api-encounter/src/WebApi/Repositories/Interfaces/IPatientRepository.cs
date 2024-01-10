// <copyright file="IPatientRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Models.Patient"/> model.
    /// </summary>
    internal interface IPatientRepository
    {
        /// <summary>
        /// Returns <see cref="Models.Patient"/> for a specific patient.
        /// </summary>
        /// <param name="patientId">Identifier of the patient.</param>
        /// <returns>A <see cref="Models.Patient"/> instance.</returns>
        Task<Models.Patient> GetPatientByIdAsync(string patientId);

        /// <summary>
        /// Returns a collection of <see cref="Models.Patient"/> with specified IDs.
        /// </summary>
        /// <param name="patientIds">Identifiers of patients.</param>
        /// <returns>A collection of <see cref="Models.Patient"/> instances.</returns>
        Task<IEnumerable<Models.Patient>> GetPatientsByIdsAsync(string[] patientIds);

        /// <summary>
        /// Returns a collection of Photos.
        /// </summary>
        /// <param name="request">Identifiers of patients photo and its Type.</param>
        /// <returns>A collection of Patient Photos.</returns>
        Task<List<PatientPhoto>> GetPatientsPhotoByIdsAsync(PatientPhotoByParam request);
    }
}
