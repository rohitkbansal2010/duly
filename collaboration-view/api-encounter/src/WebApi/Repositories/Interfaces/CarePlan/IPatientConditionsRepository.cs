// <copyright file="IPatientConditionsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan
{
    /// <summary>
    /// Repository for working on <see cref="Models.CarePlan.PatientConditions"/> model.
    /// </summary>
    internal interface IPatientConditionsRepository
    {
        Task<IEnumerable<long>> PostPatientConditionsAsync(Models.CarePlan.PatientConditions request, IDbTransaction transaction = null);
        Task<IEnumerable<Models.CarePlan.GetPatientConditionByPatientPlanIdModel>> GetConditionByPatientPlanId(long patientPlanId);
    }
}