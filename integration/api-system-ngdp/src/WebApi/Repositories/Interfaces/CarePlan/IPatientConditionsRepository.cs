// <copyright file="IPatientConditionsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces.CarePlan
{
    /// <summary>
    /// Repository for working on <see cref="PatientConditions"/> entities.
    /// </summary>
    public interface IPatientConditionsRepository
    {
        /// <summary>
        /// Get Care Plan Details.
        /// </summary>
        /// <param name="patientPlanId">Patient Plan Identifier.</param>
        /// <returns>List items of <see cref="GetPatientConditions"/>.</returns>
        Task<IEnumerable<GetPatientConditions>> GetPatientConditionsAsync(long patientPlanId);

        /// <summary>
        /// Save all items of <see cref="PatientConditions"/> which match with the filter.
        /// </summary>
        /// <param name="request">Identifier of clinic.</param>
        /// <returns>Filtered items of <see cref="PatientConditions"/>.</returns>
        Task<long> PostPatientConditionsAsync(PatientConditions request);
    }
}
