// <copyright file="IGetSlotsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    internal interface IGetSlotsRepository
    {
        /// <summary>
        /// Gets collection of <see cref="Models.ScheduleDate"/> by parameters.
        /// </summary>
        /// <param name="providerId">Provider identifier in format "prefix|id".</param>
        /// <param name="departmentId">Department identifier in format "prefix|id".</param>
        /// <param name="visitTypeId">VisitType identifier in format "prefix|id".</param>
        /// <param name="startDate">Start date of searching period.</param>
        /// <param name="endDate">End date of searching period.</param>
        /// <returns>Collection of <see cref="Models.ScheduleDate"/>.</returns>
        public Task<Models.CheckOut.ScheduleDate[]> GetOpenScheduleDatesAsync(string providerId, string departmentId, string visitTypeId, DateTimeOffset startDate, DateTimeOffset endDate);
    }
}
