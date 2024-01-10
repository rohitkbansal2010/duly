// <copyright file="IConditionRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan
{
    /// <summary>
    /// Repository for working on <see cref="Models.CarePlan.Condition"/> model.
    /// </summary>
    public interface IConditionRepository
    {
        Task<IEnumerable<Models.CarePlan.Condition>> GetConditions();
    }
}