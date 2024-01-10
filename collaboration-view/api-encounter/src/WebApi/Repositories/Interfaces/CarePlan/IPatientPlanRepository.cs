// <copyright file="IPatientPlanRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan
{
    /// <summary>
    /// Repository for working on <see cref="Models.CarePlan.PatientPlan"/> model.
    /// </summary>
    internal interface IPatientPlanRepository
    {
        Task<int> PostPatientPlanAsync(PatientPlan request);
        Task<bool> UpdatePatientPlanStatusByIdAsync(long id);
        Task<GetPatientPlanByPatientIdModel> GetPatientPlanIdByPatientIdAsync(string patientId);
        Task<long> UpdateFlourishStatementAsync(Contracts.CarePlan.UpdateFlourishStatementRequest request);
        Task<GetHealthPlanStats> GetHealthPlanStatsByPatientPlanIdAsync(long patientPlanId);
    }
}