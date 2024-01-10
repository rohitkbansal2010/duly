// <copyright file="IPatientRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Patient"/> entities.
    /// </summary>
    public interface IPatientRepository
    {
        /// <summary>
        /// Retrieve a <see cref="Patient"/> with details by specific Id.
        /// </summary>
        /// <param name="patientId">Identifier of patient.</param>
        /// <returns><see cref="Patient"/> with details.</returns>
        public Task<Patient> GetPatientByIdAsync(string patientId);

        /// <summary>
        /// Returns all available items of <see cref="Patient"/> with details for a set of identifiers.
        /// </summary>
        /// <param name="identifiers">Identifiers of patients.</param>
        /// <returns>Returns patients with details.</returns>
        public Task<IEnumerable<Patient>> GetPatientsByIdentifiersAsync(string[] identifiers);

        /// <summary>
        /// Returns all available items of <see cref="Patient"/> with details for a set of identifiers.
        /// </summary>
        /// <param name="identifiers">Identifiers of patients.</param>
        /// <returns>Returns patients with details.</returns>
        public Task<List<PatientPhoto>> GetPatientsPhotoByIdAsync(PatientPhotoByParam identifiers);
    }
}