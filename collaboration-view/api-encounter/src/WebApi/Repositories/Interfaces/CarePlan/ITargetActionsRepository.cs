// <copyright file="ITargetActionsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan
{
    /// <summary>
    /// Repository for working on <see cref="Models.CarePlan.TargetActions"/> model.
    /// </summary>
    public interface ITargetActionsRepository
    {
        Task<IEnumerable<Models.CarePlan.TargetActions>> GetTargetActionsByConditionIdAndTargetIdAsync(long conditionId, long targetId);
    }
}