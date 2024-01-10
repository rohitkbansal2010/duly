// <copyright file="IConditionRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces.CarePlan
{
    public interface IConditionRepository
    {
        /// <summary>
        /// Get all conditions.
        /// </summary>
        /// <returns>List items of <see cref="Condition"/>.</returns>
        Task<IEnumerable<Condition>> GetConditionsAsync();
    }
}