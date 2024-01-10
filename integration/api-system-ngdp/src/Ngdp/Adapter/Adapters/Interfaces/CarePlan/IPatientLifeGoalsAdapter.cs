// <copyright file="IPatientLifeGoalsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models.CarePlan;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan
{
    /// <summary>
    /// An adapter for working with "PatientLifeGoal" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface IPatientLifeGoalsAdapter
    {
        /// <summary>
        /// Get Patient Life Goals.
        /// </summary>
        /// <returns>List items of <see cref="GetPatientLifeGoals"/>.</returns>
        Task<IEnumerable<GetPatientLifeGoals>> GetPatientLifeGoalsAsync(long patientPlanId);

        /// <summary>
        /// Post items in <see cref="PatientLifeGoals"/>.
        /// </summary>
        /// <param name="request">Details to be posted.</param>
        /// <param name="transaction">To post multiple entries in one go.</param>
        /// <returns>Id from table of <see cref="PatientLifeGoals"/>.</returns>
        Task<List<PatientLifeGoalResponse>> PostOrUpdateLifeGoalAsync(IEnumerable<PatientLifeGoals> request, IDbTransaction transaction = null);

        /// <summary>
        /// Finds matching items of <see cref="PatientLifeGoals"/>.
        /// </summary>
        /// <param name="id">Criteria to search by.</param>
        /// <returns>Patient LifeGoal id of <see cref="PatientLifeGoals"/>.</returns>
        Task<long> DeletePatientLifeGoalAsync(long id);
    }
}
