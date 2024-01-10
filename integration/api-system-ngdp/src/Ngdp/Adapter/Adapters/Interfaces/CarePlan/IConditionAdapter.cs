// <copyright file="IConditionAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan
{
    /// <summary>
    /// An adapter for working with "Condition" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface IConditionAdapter
    {
        /// <summary>
        /// Get all conditions.
        /// </summary>
        /// <returns>List items of <see cref="Condition"/>.</returns>
        Task<IEnumerable<Condition>> GetConditionsAsync();
    }
}