// <copyright file="IPatientLifeGoalRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts.CarePlan;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces.CarePlan
{
    /// <summary>
    /// Repository for working on <see cref="PatientLifeGoal"/> entities.
    /// </summary>
    public interface IPatientLifeGoalRepository
    {
        /// <summary>
        /// Save all items of <see cref="PatientLifeGoal"/> which match with the filter.
        /// </summary>
        /// <param name="request">Patient Life Goal Details.</param>
        /// <returns>Id from table of <see cref="PatientLifeGoal"/>.</returns>
        Task<int> PostPatientLifeGoalAsync(PatientLifeGoal request);
    }
}
