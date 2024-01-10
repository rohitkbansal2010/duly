// <copyright file="IConditionTargetsService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="GetConditionTargetsResponse"/> entity.
    /// </summary>
    public interface IConditionTargetsService
    {
        /// <summary>
        /// Returns all active condition targets by condition id in response.
        /// </summary>
        /// <returns>Returns list of condition targets.</returns>
        /// <param name="conditionIds">Condition Id.</param>
        Task<IEnumerable<GetConditionTargetsResponse>> GetConditionTargetsByConditionId(string conditionIds);
    }
}