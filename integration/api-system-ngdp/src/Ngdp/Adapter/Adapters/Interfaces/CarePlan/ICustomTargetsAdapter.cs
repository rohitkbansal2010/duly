// <copyright file="ICustomTargetsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models.CarePlan;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan
{
    /// <summary>
    /// An adapter for working with "CustomTargets" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface ICustomTargetsAdapter
    {
        /// <summary>
        /// Finds matching items of <see cref="CustomTargets"/>.
        /// </summary>
        /// <param name="request">Criteria to search by.</param>
        /// <returns>Filtered items of <see cref="CustomTargets"/>.</returns>
        Task<int> PostCustomTargetsAsync(CustomTargets request);
    }
}
