// <copyright file="IScheduleFollowUpRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="ScheduleFollowUp"/> entities.
    /// </summary>
    public interface IScheduleFollowUpRepository
    {
        /// <summary>
        /// Save all items of <see cref="ScheduleFollowUp"/> which match with the filter.
        /// </summary>
        /// <param name="request">Identifier of clinic.</param>
        /// <returns>Filtered items of <see cref="ScheduleFollowUp"/>.</returns>
        Task<int> PostScheduleFollowUpAsync(ScheduleFollowUp request);
    }
}