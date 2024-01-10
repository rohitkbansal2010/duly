// <copyright file="IPatientActionsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan
{
    /// <summary>
    /// Repository for working on <see cref="Models.CarePlan.PatientActions"/> model.
    /// </summary>
    internal interface IPatientActionsRepository
    {
        Task<IEnumerable<Models.CarePlan.GetPatientActions>> GetPatientActionsByPatientTargetIdAsync(long patientTargetId);
        Task<long> PostPatientActionsAsync(IEnumerable<Models.CarePlan.PatientActions> request, IDbTransaction transaction = null);
        Task<long> UpdateActionProgressAsync(Models.CarePlan.UpdateActionProgress request, long id);
        Task<Models.CarePlan.GetPatientActionStats> GetPatientActionStatsAsync(long patientPlanId);
    }
}