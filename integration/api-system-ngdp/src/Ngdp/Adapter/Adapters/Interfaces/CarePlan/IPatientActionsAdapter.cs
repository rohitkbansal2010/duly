// <copyright file="IPatientActionsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models.CarePlan;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan
{
    /// <summary>
    /// An adapter for working with "PatientActions" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface IPatientActionsAdapter
    {
        /// <summary>
        /// Get Patient Actions By Patient Target Identifier.
        /// </summary>
        /// <returns>List items of <see cref="GetPatientActions"/>.</returns>
        Task<IEnumerable<GetPatientActions>> GetPatientActionsAsync(long patientTargetId);

        /// <summary>
        /// Finds matching items of <see cref="PatientActions"/>.
        /// </summary>
        /// <param name="request">Criteria to search by.</param>
        /// <returns>Filtered items of <see cref="PatientActions"/>.</returns>
        Task<long> PostPatientActionsAsync(IEnumerable<PatientActions> request, IDbTransaction transaction = null);
    }
}
