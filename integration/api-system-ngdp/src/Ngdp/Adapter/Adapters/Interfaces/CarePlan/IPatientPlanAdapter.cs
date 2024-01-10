// <copyright file="IPatientPlanAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan
{
    /// <summary>
    /// An adapter for working with "PatientConditions" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface IPatientPlanAdapter
    {

        /// <summary>
        /// Get Care Plan Details.
        /// </summary>
        /// <returns>List items of <see cref="GetPatientPlan"/>.</returns>
        Task<IEnumerable<GetPatientPlan>> GetPatientPlanAsync(string patientId);

        /// <summary>
        /// Finds matching items of <see cref="PatientPlan"/>.
        /// </summary>
        /// <param name="request">Criteria to search by.</param>
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
