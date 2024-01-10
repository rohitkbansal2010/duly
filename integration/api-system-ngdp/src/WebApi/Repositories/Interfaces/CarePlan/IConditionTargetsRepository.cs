// <copyright file="IConditionTargetsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces.CarePlan
{
    public interface IConditionTargetsRepository
    {
        /// <summary>
        /// Get all condition targets by condition ids.
        /// </summary>
        /// <param name="conditionIds">Identifier of condition targets.</param>
        /// <returns>List items of <see cref="ConditionTargets"/>.</returns>
        Task<IEnumerable<ConditionTargets>> GetConditionTargetsByConditionIdAsync(string conditionIds);
    }
}