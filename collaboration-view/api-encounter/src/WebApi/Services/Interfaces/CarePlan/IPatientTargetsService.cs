// <copyright file="IPatientTargetsService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="PatientTargets"/> entity.
    /// </summary>
    public interface IPatientTargetsService
    {
        /// <summary>
        /// Returns patient targets patient plan id.
        /// </summary>
        /// <returns>Returns patient targets.</returns>
        /// <param name="patientPlanId">Patient Plan Identifier.</param>
        Task<IEnumerable<Contracts.CarePlan.GetPatientTargets>> GetPatientTargetsByPatientPlanIdAsync(long patientPlanId);

        /// <summary>
        /// Post patient targets and returns patientcoditionid in response.
        /// </summary>
        /// <returns>Patient condition id.</returns>
        /// <param name="request"> Patient target.</param>
        Task<long> PostPatientTargetsAsync(PatientTargets request);

        /// <summary>
        /// Delete patient target and returns patientTargetId in response.
        /// </summary>
        /// <returns>Patient Target id.</returns>
        /// <param name="id">Id Of Target which is to be deleted.</param>
        Task<int> DeletePatientTargetAsync(long id);

        /// <summary>
        /// Updates Review Status of patient targets and returns patientTargetId in response.
        /// </summary>
        /// <returns>Patient Target id.</returns>
        /// <param name="request">Model for Review status.</param>
        /// <param name="patientTargetId">Id Of Target which is to be deleted.</param>
        Task<long> UpdatePatientTargetReviewStatusAsync(UpdatePatientTargetReviewStatus request, long patientTargetId);

        /// <summary>
        /// Delete patient target and returns patientTargetId in response.
        /// </summary>
        /// <returns>Patient Target id.</returns>
        /// <param name="request">Model for Updating Patient Target.</param>
        /// <param name="patientTargetId">Id Of Target to be updated.</param>
        Task<long> UpdatePatientTargetsAsync(UpdatePatientTargets request, long patientTargetId);
    }
}