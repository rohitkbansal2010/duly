// <copyright file="IPatientLifeGoalAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models.CarePlan;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan
{
    /// <summary>
    /// An adapter for working with "PatientLifeGoal" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface IPatientLifeGoalAdapter
    {
        /// <summary>
        /// Finds matching items of <see cref="PatientLifeGoal"/>.
        /// </summary>
        /// <param name="request">Details to be posted.</param>
        /// <returns>Id from table of <see cref="PatientLifeGoal"/>.</returns>
        Task<int> PostPatientLifeGoalAsync(PatientLifeGoal request);
    }
}
