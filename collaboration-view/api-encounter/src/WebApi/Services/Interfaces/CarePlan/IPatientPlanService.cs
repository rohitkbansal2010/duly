// <copyright file="IPatientPlanService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.CarePlan.PatientPlan"/> entity.
    /// </summary>
    public interface IPatientPlanService
    {
        /// <summary>
        /// Returns Patient Plan data saved response.
        /// </summary>
        /// <param name="request"><see cref="Contracts.CarePlan.PatientPlan"/>PatientPlan.</param>
        /// <returns>Returns Id.</returns>
        Task<int> PostPatientPlanAsync(PatientPlan request);

        /// <summary>
        /// Returns Patient Plan status Update response.
        /// </summary>
        /// <param name="id">PatientPlanId.</param>
        /// <returns>Returns bool response.</returns>
        Task<bool> UpdatePatientPlanStatusByIdAsync(long id);

        /// <summary>
        /// Returns Patient Plan by patient plan id.
        /// </summary>
        /// <param name="id">PatientPlanId.</param>
        /// <returns>GetPatientPlanByIdResponse response.</returns>
        Task<GetPatientPlanByPatientId> GetPatientPlanByPatientIdAsync(string id);

        /// <summary>
        /// Updates Flourish Statement in Patient Plan Table by patient plan id.
        /// </summary>
        /// <param name="request"><see cref="Contracts.CarePlan.UpdateFlourishStatementRequest"/>Encapsulates Patient Plan Id and Flourish Statement.</param>
        /// <returns>GetPatientPlanByIdResponse response.</returns>
        Task<long> UpdateFlourishStatementAsync(UpdateFlourishStatementRequest request);

        /// <summary>
        /// Get Count for Patient Conditions, Patient Targets and Patient Actions.
        /// </summary>
        /// <param name="patientPlanId">Patient Plan Identifier.</param>
        /// <returns>GetPatientPlanByIdResponse response.</returns>
        Task<GetHealthPlanStats> GetHealthPlanStatsByPatientPlanIdAsync(long patientPlanId);
    }
}