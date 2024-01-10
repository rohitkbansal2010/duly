// <copyright file="ICustomTargetsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts.CarePlan;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces.CarePlan
{
    /// <summary>
    /// Repository for working on <see cref="CustomTargets"/> entities.
    /// </summary>
    public interface ICustomTargetsRepository
    {
        /// <summary>
        /// Save all items of <see cref="CustomTargets"/> which match with the filter.
        /// </summary>
        /// <param name="request">Identifier of clinic.</param>
        /// <returns>Filtered items of <see cref="CustomTargets"/>.</returns>
        Task<int> PostCustomTargetsAsync(CustomTargets request);
    }
}
