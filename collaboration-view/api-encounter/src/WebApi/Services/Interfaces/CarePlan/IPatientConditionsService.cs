// <copyright file="IPatientConditionsService.cs" company="Duly Health and Care">
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
    /// the corresponding contract for the <see cref="Contracts.CarePlan.PatientConditions"/> entity.
    /// </summary>
    public interface IPatientConditionsService
    {
        /// <summary>
        /// Returns Patient Conditions data saved response.
        /// </summary>
        /// <param name="request"><see cref="Contracts.CarePlan.PatientConditions"/>PatientConditions.</param>
        /// <returns>Returns Id.</returns>
        Task<IEnumerable<long>> PostPatientConditionsAsync(PatientConditions request);

        /// <summary>
        /// Returns all active conditions by id in response.
        /// </summary>
        /// <returns>Returns list of conditions.</returns>
        /// <param name="id">Patient plan id.</param>
        Task<IEnumerable<Contracts.CarePlan.GetPatientConditionByPatientPlanId>> GetConditionByPatientPlanId(long id);
    }
}