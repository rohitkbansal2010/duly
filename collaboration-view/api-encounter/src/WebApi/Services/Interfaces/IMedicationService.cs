// <copyright file="IMedicationService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.Medication"/> entity.
    /// </summary>
    public interface IMedicationService
    {
        /// <summary>
        /// Retrieve <see cref="Contracts.Medications"/> that represents information
        /// about medications (<see cref="Contracts.Medication"/>)
        /// that the specific patient is/was taking, divided by the type of schedule.
        /// </summary>
        /// <param name="patientId">The identifier of a specific patient.</param>
        /// <returns>An instance of <see cref="Contracts.Medications"/> for a specific patient.</returns>
        Task<Contracts.Medications> GetMedicationsByPatientIdAsync(string patientId);

        /// <summary>
        /// Retrieve <see cref="Contracts.Medications"/> that represents information
        /// about medications (<see cref="Contracts.Medication"/>)
        /// that the specific patient is/was taking, divided by the type of schedule.
        /// </summary>
        /// <param name="patientId">The identifier of a specific patient.</param>
        /// <param name="appointmentId">The identifier of a specific appointment.</param>
        /// <returns>An instance of <see cref="Contracts.Medications"/> for a specific patient.</returns>
        Task<Contracts.Medications> GetMedicationsRequestByPatientIdAsync(string patientId, string appointmentId);
    }
}
