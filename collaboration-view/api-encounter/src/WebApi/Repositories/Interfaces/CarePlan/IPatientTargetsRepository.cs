// <copyright file="IPatientTargetsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan
{
    /// <summary>
    /// Repository for working on <see cref="PatientTargets"/> model.
    /// </summary>
    public interface IPatientTargetsRepository
    {
        Task<IEnumerable<Models.CarePlan.GetPatientTargets>> GetPatientTargetsByPatientPlanIdAsync(long patientPlanId);

        Task<long> PostPatientTargetsAsync(PatientTargets request);

        Task<int> DeletePatientTargetAsync(long id);

        Task<long> UpdatePatientTargetReviewStatusAsync(UpdatePatientTargetReviewStatus request, long patientTargetId);

        Task<long> UpdatePatientTargetsAsync(UpdatePatientTargets request, long patientTargetId);
    }
}