// <copyright file="IMedicationRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Medication"/> entities.
    /// </summary>
    public interface IMedicationRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="Medication"/> which match with the filter.
        /// </summary>
        /// <param name="patientId">Identifier of Patient.</param>
        /// <param name="status">Status of the medication which should be included. If null, then returns every entry.</param>
        /// <param name="medicationCategories">Medication categories that should be included, if null or empty includes all.</param>
        /// <returns>Filtered items of <see cref="Medication"/>.</returns>
        Task<IEnumerable<Medication>> FindMedicationsForPatientAsync(string patientId, MedicationStatus? status = null, MedicationCategory[] medicationCategories = null);

        /// <summary>
        /// Retrieve all items of <see cref="Medication"/> which match with the filter.
        /// </summary>
        /// <param name="patientId">Identifier of Patient.</param>
        /// <param name="appointmentId">Identifier of appointment.</param>
        /// <param name="status">Status of the medication which should be included. If null, then returns every entry.</param>
        /// <param name="medicationCategories">Medication categories that should be included, if null or empty includes all.</param>
        /// <returns>Filtered items of <see cref="Medication"/>.</returns>
        Task<IEnumerable<Medication>> FindMedicationsRequestForPatientAsync(string patientId, string appointmentId, MedicationStatus? status = null, MedicationCategory[] medicationCategories = null);
    }
}