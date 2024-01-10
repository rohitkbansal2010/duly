// <copyright file="ICustomActionsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts.CarePlan;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces.CarePlan
{
    /// <summary>
    /// Repository for working on <see cref="CustomActions"/> entities.
    /// </summary>
    public interface ICustomActionsRepository
    {
        /// <summary>
        /// Save all items of <see cref="CustomActions"/> which match with the filter.
        /// </summary>
        /// <param name="request">Identifier of clinic.</param>
        /// <returns>Filtered items of <see cref="CustomActions"/>.</returns>
        Task<int> PostCustomActionsAsync(CustomActions request);
    }
}
