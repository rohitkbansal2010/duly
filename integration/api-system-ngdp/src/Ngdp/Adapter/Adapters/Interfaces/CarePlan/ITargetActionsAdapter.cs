// <copyright file="ITargetActionsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan
{
    /// <summary>
    /// An adapter for working with "TargetActions" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface ITargetActionsAdapter
    {
        /// <summary>
        /// Get all target action by target id.
        /// </summary>
        /// <param name="targetId">Identifier of target actions.</param>
        /// <returns>List items of <see cref="TargetActions"/>.</returns>
        Task<IEnumerable<TargetActions>> GetTargetActionsByTargetIdAsync(long targetId);
    }
}