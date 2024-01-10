// <copyright file="ICustomActionsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan
{
    /// <summary>
    /// Repository for working on <see cref="Models.CarePlan.CustomActions"/> model.
    /// </summary>
    internal interface ICustomActionsRepository
    {
        Task<long> PostCustomActionsAsync(Models.CarePlan.CustomActions request);
    }
}