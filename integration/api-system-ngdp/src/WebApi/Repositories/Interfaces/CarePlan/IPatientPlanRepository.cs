// <copyright file="IPatientPlanRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces.CarePlan
{
    /// <summary>
    /// Repository for working on <see cref="PatientPlan"/> entities.
    /// </summary>
    public interface IPatientPlanRepository
    {
        /// <summary>
        /// Get Patient Plan Details.
        /// </summary>
        /// <param name="patientId">Patient Identifier.</param>
        /// <returns>List items of <see cref="GetPatientPlan"/>.</returns>
        Task<IEnumerable<GetPatientPlan>> GetPatientPlanAsync(string patientId);

        /// <summary>
        /// Save all items of <see cref="PatientPlan"/> which match with the filter.
        /// </summary>
        /// <param name="request">Identifier of clinic.</param>
        /// <returns>Filtered items of <see cref="PatientPlan"/>.</returns>
        Task<int> PostPatientPlanAsync(PatientPlan request);

        /// <summary>
        /// Updates patient plan status by patient plan id.
        /// </summary>
        /// <param name="id">Patient Plan Id.</param>
        /// <returns>Returns status of Patient Plan.</returns>
        Task<bool> UpdatePatientPlanStatusByIdAsync(long id);
    }
}