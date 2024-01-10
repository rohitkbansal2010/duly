// <copyright file="IPatientTargetsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan
{
    /// <summary>
    /// An adapter for working with "PatientTargets" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface IPatientTargetsAdapter
    {
        /// <summary>
        /// Get Patient Targets by Patient Plan.
        /// </summary>
        /// <returns>List items of <see cref="GetPatientTargets"/>.</returns>
        Task<IEnumerable<GetPatientTargets>> GetPatientTargetsAsync(long patientPlanId);

        /// <summary>
        /// Finds matching items of <see cref="PatientTarget"/>.
        /// </summary>
        /// <param name="request">Criteria to search by.</param>
        /// <returns>Patient condition id of <see cref="PatientTarget"/>.</returns>
        Task<int> PostPatientTargetAsync(PatientTarget request);

        /// <summary>
        /// Finds matching items of <see cref="PatientTarget"/>.
        /// </summary>
        /// <param name="id">Criteria to search by.</param>
        /// <returns>Patient Target id of <see cref="PatientTarget"/>.</returns>
        Task<int> DeletePatientTargetAsync(long id);
    }
}
