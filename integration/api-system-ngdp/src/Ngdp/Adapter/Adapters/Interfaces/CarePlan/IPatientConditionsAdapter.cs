// <copyright file="IPatientConditionsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models.CarePlan;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan
{
    /// <summary>
    /// An adapter for working with "PatientConditions" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface IPatientConditionsAdapter
    {
        /// <summary>
        /// Get Patient Conditions.
        /// </summary>
        /// <returns>List items of <see cref="GetPatientConditions"/>.</returns>
        Task<IEnumerable<GetPatientConditions>> GetPatientConditionsAsync(long patientPlanId);

        /// <summary>
        /// Finds matching items of <see cref="PatientConditions"/>.
        /// </summary>
        /// <param name="request">Criteria to search by.</param>
        /// <param name="transaction">To post multiple entries in one go.</param>
        /// <returns>Filtered items of <see cref="PatientConditions"/>.</returns>
        Task<long> PostPatientConditionsAsync(PatientConditions request, IDbTransaction transaction = null);
    }
}
