// <copyright file="ICustomActionsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models.CarePlan;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan
{
    /// <summary>
    /// An adapter for working with "CustomActions" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface ICustomActionsAdapter
    {
        /// <summary>
        /// Finds matching items of <see cref="CustomActions"/>.
        /// </summary>
        /// <param name="request">Criteria to search by.</param>
        /// <returns>Filtered items of <see cref="CustomActions"/>.</returns>
        Task<int> PostCustomTargetsAsync(CustomActions request);
    }
}
