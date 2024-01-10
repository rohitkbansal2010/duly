// <copyright file="ITargetActionsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces.CarePlan
{
    public interface ITargetActionsRepository
    {
        /// <summary>
        /// Get all target action by target id.
        /// </summary>
        /// <param name="targetId">Identifier of target actions.</param>
        /// <returns>List items of <see cref="TargetActions"/>.</returns>
        Task<IEnumerable<TargetActions>> GetTargetActionsByTargetIdAsync(long targetId);
    }
}