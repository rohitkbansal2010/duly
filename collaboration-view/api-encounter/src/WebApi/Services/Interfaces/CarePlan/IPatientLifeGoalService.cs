// <copyright file="IPatientLifeGoalService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.CarePlan.PatientLifeGoal"/> entity.
    /// </summary>
    public interface IPatientLifeGoalService
    {
        /// <summary>
        /// Returns Patient LifeGoal data saved response.
        /// </summary>
        /// <param name="request"><see cref="Contracts.CarePlan.PatientLifeGoal"/>PatientPlan.</param>
        /// <returns>Returns Id.</returns>
        Task<PostOrUpdatePatientLifeGoalResponse> PostOrUpdateLifeGoalAsync(PostRequestForLifeGoals request);

        /// <summary>
        /// Delete patient lifegoal and returns patientLifeGoalId in response.
        /// </summary>
        /// <returns>Patient LifeGoal id.</returns>
        /// <param name="patientLifeGoalId">Id Of LifeGoal which is to be deleted.</param>
        Task<long> DeletePatientLifeGoalAsync(long patientLifeGoalId);

        /// <summary>
        /// Get patient life goal in response.
        ///  </summary>
        /// <returns>Patient LifeGoal id.</returns>
        /// <param name="id">Id Of LifeGoal.</param>
        Task<IEnumerable<GetPatientLifeGoalByPatientPlanId>> GetPatientLifeGoalByPatientPlanIdAsync(long id);

        /// <summary>
        /// Returns Patient LifeGoalTargetMapping data saved response.
        /// </summary>
        /// <param name="request"><see cref="Contracts.CarePlan.PatientLifeGoalTargetMapping"/>PatientPlan.</param>
        /// <returns>Returns Id.</returns>
        Task<long> PostPatientLifeGoalTargetMappingAsync(long patientTargetId, IEnumerable<long> patientLifeGoalIds);

        /// <summary>
        /// Returns patient life goal target mapping by patient target id.
        /// </summary>
        /// <returns>Returns patient actions.</returns>
        /// <param name="patientTargetId">Patient Target Identifier.</param>
        Task<IEnumerable<GetPatientLifeGoalTargetMapping>> GetPatientLifeGoalTargetMappingByPatientIdAsync(long patientTargetId);


        /// <summary>
        /// Returns Patient Life Goals which are attached to Health Plan and Patient Actions mapped to these life goals.
        /// </summary>
        /// <returns>Returns patient life goals and respective patient actions.</returns>
        /// <param name="patientPlanId">Patient Plan Identifier.</param>
        Task<GetPatientLifeGoalAndActionTrackingResponse> GetPatientLifeGoalAndActionTrackingAsync(long patientPlanId);
    }
}
