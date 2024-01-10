// <copyright file="IPatientActionsService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.CarePlan.PatientActions"/> entity.
    /// </summary>
    public interface IPatientActionsService
    {
        /// <summary>
        /// Returns patient actions by patient target id.
        /// </summary>
        /// <returns>Returns patient actions.</returns>
        /// <param name="patientTargetId">Patient Target Identifier.</param>
        Task<IEnumerable<GetPatientActions>> GetPatientActionsByPatientTargetIdAsync(long patientTargetId);

        /// <summary>
        /// Returns Patient Actions data saved response.
        /// </summary>
        /// <param name="request"><see cref="Contracts.CarePlan.PatientActions"/>PatientActions.</param>
        /// <returns>Returns Id.</returns>
        Task<long> PostPatientActionsAsync(IEnumerable<PatientActions> request);

        /// <summary>
        /// Update patient action and returns patietActionId in response.
        /// </summary>
        /// <returns>Patient Target id.</returns>
        /// <param name="request">Id Of Target which is to be deleted.</param>
        /// <param name="patientActionId">Id Of patientAction which is to be updated.</param>
        Task<long> UpdateActionProgressAsync(UpdateActionProgress request, long patientActionId);

        /// <summary>
        /// Get Patient Actions statistics by patient plan id.
        /// </summary>
        /// <returns>Return Statistics</returns>
        /// <param name="patientPlanId">Patient plan identifier.</param>
        Task<GetPatientActionStats> GetPatientActionStatsAsync(long patientPlanId);
    }
}