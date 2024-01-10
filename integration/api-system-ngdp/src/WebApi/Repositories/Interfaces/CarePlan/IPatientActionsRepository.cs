// <copyright file="IPatientActionsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces.CarePlan
{
    /// <summary>
    /// Repository for working on <see cref="PatientActions"/> entities.
    /// </summary>
    public interface IPatientActionsRepository
    {

        /// <summary>
        /// Get Patient Actions by Patient Target Identifier.
        /// </summary>
        /// <param name="patientTargetId">Patient Target Identifier.</param>
        /// <returns>List items of <see cref="GetPatientActions"/>.</returns>
        Task<IEnumerable<GetPatientActions>> GetPatientActionsAsync(long patientTargetsId);

        /// <summary>
        /// Save all items of <see cref="PatientActions"/> which match with the filter.
        /// </summary>
        /// <param name="request">Identifier of clinic.</param>
        /// <returns>Filtered items of <see cref="PatientActions"/>.</returns>
        Task<long> PostPatientActionsAsync(IEnumerable<PatientActions> request);
    }
}
