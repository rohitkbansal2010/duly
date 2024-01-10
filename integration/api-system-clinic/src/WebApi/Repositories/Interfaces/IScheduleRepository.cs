// <copyright file="IScheduleRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on schedule entities.
    /// </summary>
    public interface IScheduleRepository
    {
        /// <summary>
        /// Retrieve a collection of <see cref="ScheduleDay"/> with details by specific parameters.
        /// </summary>
        /// <param name="startDate">Start date of requested slots.</param>
        /// <param name="endDate">End date of requested slots.</param>
        /// <param name="providerId">Provider identifier.</param>
        /// <param name="departmentId">Department identifier.</param>
        /// <param name="visitTypeId">Visit type identifier.</param>
        /// <returns>Collection of <see cref="ScheduleDay"/> with details.</returns>
        public Task<IEnumerable<ScheduleDay>> GetScheduleAsync(DateTime startDate, DateTime endDate, string providerId, string departmentId, string visitTypeId);
    }
}
