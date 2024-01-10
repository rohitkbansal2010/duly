// <copyright file="IMedicationRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Models.Medication"/> model.
    /// </summary>
    internal interface IMedicationRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="Models.Medication"/> that match the filtering parameters.
        /// </summary>
        /// <param name="patientId">Identifier of a specific patient.</param>
        /// <param name="medicationStatus">Medication status <see cref="Models.MedicationStatus"/>.</param>
        /// <param name="medicationCategories">Medication categories <see cref="Models.MedicationCategory"/>.</param>
        /// <returns>Filtered items of <see cref="Models.Medication"/>.</returns>
        Task<IEnumerable<Models.Medication>> GetMedicationsAsync(string patientId, Models.MedicationStatus medicationStatus, IEnumerable<Models.MedicationCategory> medicationCategories);

        /// <summary>
        /// Retrieve all items of <see cref="Models.Medication"/> that match the filtering parameters.
        /// </summary>
        /// <param name="patientId">Identifier of a specific patient.</param>
        /// <param name="appointmentId">Identifier of a specific appointmentId.</param>
        /// <param name="medicationStatus">Medication status <see cref="Models.MedicationStatus"/>.</param>
        /// <param name="medicationCategories">Medication categories <see cref="Models.MedicationCategory"/>.</param>
        /// <returns>Filtered items of <see cref="Models.Medication"/>.</returns>
        Task<IEnumerable<Models.Medication>> GetMedicationsRequestAsync(string patientId, string appointmentId, Models.MedicationStatus medicationStatus, IEnumerable<Models.MedicationCategory> medicationCategories);
    }
}
