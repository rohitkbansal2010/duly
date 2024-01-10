// <copyright file="ICustomActionsService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.CarePlan.CustomActions"/> entity.
    /// </summary>
    public interface ICustomActionsService
    {
        /// <summary>
        /// Returns Custom actions data saved response.
        /// </summary>
        /// <param name="request"><see cref="Contracts.CarePlan.CustomActions"/>CustomActions.</param>
        /// <returns>Returns Id.</returns>
        Task<long> PostCustomActionsAsync(CustomActions request);
    }
}