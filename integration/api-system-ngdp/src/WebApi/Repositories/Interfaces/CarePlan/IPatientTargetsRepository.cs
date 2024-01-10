// <copyright file="IPatientTargetsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces.CarePlan
{
    /// <summary>
    /// Repository for working on <see cref="PatientTarget"/> entities.
    /// </summary>
    public interface IPatientTargetsRepository
    {
        /// <summary>
        /// Get Patient Targets.
        /// </summary>
        /// <param name="patientPlanId">Patient Plan Identifier.</param>
        /// <returns>List items of <see cref="GetPatientTargets"/>.</returns>
        Task<IEnumerable<GetPatientTargets>> GetPatientTargetsAsync(long patientPlanId);

        /// <summary>
        /// Save all items of <see cref="PatientTarget"/>.
        /// </summary>
        /// <param name="request">Patient targets.</param>
        Task<int> PostPatientTargetsAsync(PatientTarget request);

        /// <summary>
        /// Post True in Deleted column  of <see cref="PatientTarget"/>.
        /// </summary>
        /// <param name="id">Patient targets.</param>
        Task<int> DeletePatientTargetAsync(long id);
    }
}