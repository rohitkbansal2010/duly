// <copyright file="IConditionService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.CarePlan.Condition"/> entity.
    /// </summary>
    public interface IConditionService
    {
        /// <summary>
        /// Returns all active conditions in response.
        /// </summary>
        /// <returns>Returns list of conditions.</returns>
        Task<IEnumerable<Contracts.CarePlan.GetConditionResponse>> GetConditions();
    }
}