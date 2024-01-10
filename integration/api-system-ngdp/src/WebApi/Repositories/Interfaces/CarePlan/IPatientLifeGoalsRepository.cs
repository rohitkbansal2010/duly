// <copyright file="IPatientLifeGoalsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces.CarePlan
{
    /// <summary>
    /// Repository for working on <see cref="PatientLifeGoals"/> entities.
    /// </summary>
    public interface IPatientLifeGoalsRepository
    {
        /// <summary>
        /// Get Care Plan Details.
        /// </summary>
        /// <param name="patientPlanId">Patient Plan Identifier.</param>
        /// <returns>List items of <see cref="GetPatientLifeGoals"/>.</returns>
        Task<IEnumerable<GetPatientLifeGoals>> GetPatientLifeGoalsAsync(long patientPlanId);

        /// <summary>
        /// Save all items of <see cref="PatientLifeGoals"/> which match with the filter.
        /// </summary>
        /// <param name="request">Patient Life Goal Details.</param>
        /// <returns>Id from table of <see cref="PatientLifeGoals"/>.</returns>
        Task<List<PatientLifeGoalResponse>> PostOrUpdateLifeGoalAsync(IEnumerable<PatientLifeGoals> request);

        /// <summary>
        /// Post True in Deleted column  of <see cref="PatientLifeGoals"/>.
        /// </summary>
        /// <param name="id">Patient LifeGoal Id.</param>
        Task<long> DeletePatientLifeGoalAsync(long id);
    }
}
