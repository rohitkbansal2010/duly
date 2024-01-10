// <copyright file="ITargetActionsService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="TargetActions"/> entity.
    /// </summary>
    public interface ITargetActionsService
    {
        /// <summary>
        /// Returns all active target actions by target id in response.
        /// </summary>
        /// <returns>Returns list of target actions.</returns>
        /// <param name="conditionId">Condition Id.</param>
        /// <param name="targetId">Target Id.</param>
        Task<IEnumerable<TargetActions>> GetTargetActionsByConditionIdAndTargetIdAsync(long conditionId, long targetId);
    }
}