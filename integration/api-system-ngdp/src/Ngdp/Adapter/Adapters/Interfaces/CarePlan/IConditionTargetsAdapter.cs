// <copyright file="IConditionTargetsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan
{
    /// <summary>
    /// An adapter for working with "ConditionTargets" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface IConditionTargetsAdapter
    {
        /// <summary>
        /// Get all condition targets by condition ids.
        /// </summary>
        /// <param name="conditionIds">Identifier of condition targets.</param>
        /// <returns>List items of <see cref="ConditionTargets"/>.</returns>
        Task<IEnumerable<ConditionTargets>> GetConditionTargetsByConditionIdAsync(string conditionIds);
    }
}