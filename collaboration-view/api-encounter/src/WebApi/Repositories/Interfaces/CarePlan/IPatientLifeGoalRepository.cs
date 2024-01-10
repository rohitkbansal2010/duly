// <copyright file="IPatientLifeGoalRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan
{
    /// <summary>
    /// Repository for working on <see cref="Models.CarePlan.PatientLifeGoal"/> model.
    /// </summary>
    internal interface IPatientLifeGoalRepository
    {
        Task<PostOrUpdatePatientLifeGoalResponse> PostOrUpdateLifeGoalAsync(PostRequestForLifeGoals request, IDbTransaction transaction = null);

        Task<long> DeletePatientLifeGoalAsync(long patientLifeGoalId);

        Task<IEnumerable<GetPatientLifeGoalByPatientPlanIdModel>> GetPatientLifeGoalByPatientPlanIdAsync(long id);

        Task<long> PostPatientLifeGoalTargetMappingAsync(long patientTargetId, IEnumerable<long> patientLifeGoalIds, IDbTransaction transaction = null);

        Task<IEnumerable<GetPatientLifeGoalTargetMapping>> GetPatientLifeGoalTargrtMappingByPatientIdAsync(long patientTargetId);

        Task<IEnumerable<PatientLifeGoalAndActionTracking>> GetPatientLifeGoalAndActionTrackingAsync(long patientPlanId);
    }
}
