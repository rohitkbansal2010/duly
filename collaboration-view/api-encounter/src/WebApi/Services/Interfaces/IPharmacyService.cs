// <copyright file="IPharmacyService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.Pharmacy"/> entity.
    /// </summary>
    public interface IPharmacyService
    {
        /// <summary>
        /// Retrieve <see cref="Contracts.Pharmacy"/> that represents information
        /// about medications (<see cref="Contracts.Pharmacy"/>)
        /// that the specific patient is/was taking, divided by the type of schedule.
        /// </summary>
        /// <param name="patientId">The identifier of a specific patient.</param>
        /// <returns>An instance of <see cref="Contracts.Pharmacy"/> for a specific patient.</returns>
        Task<Contracts.Pharmacy> GeTPreferredPharmacyByPatientIdAsync(string patientId);
    }
}
