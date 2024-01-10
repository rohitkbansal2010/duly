// <copyright file="IGetSlotsService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.ScheduleDate"/> entity.
    /// </summary>
    public interface IGetSlotsservice
    {
        /// <summary>
        /// Get schedule dates with time slots by parameters.
        /// </summary>
        /// <param name="visitTypeId">Visit type identifier.</param>
        /// <param name="appointmentId">appointment identifier in format "id".</param>
        /// <param name="startDate">Start date of searching period.</param>
        /// <param name="endDate">End date of searching period.</param>
        /// <returns>Array of <see cref="ScheduleDate"/> with collection of <see cref="TimeSlot"/> inside.</returns>
        public Task<IEnumerable<ScheduleDate>> GetScheduleDateAsync(string visitTypeId, string appointmentId, DateTimeOffset startDate, DateTimeOffset endDate);

        /// <summary>
        /// Get schedule dates with time slots by parameters.
        /// </summary>
        /// <param name="visitTypeId">Visit type identifier.</param>
        /// <param name="departmentId">Department identifier.</param>
        /// <param name="providerId">provider identifier.</param>
        /// <param name="startDate">Start date of searching period.</param>
        /// <param name="endDate">End date of searching period.</param>
        /// <returns>Array of <see cref="ScheduleDate"/> with collection of <see cref="TimeSlot"/> inside.</returns>
        public Task<IEnumerable<ScheduleDate>> GetReferralScheduleDateAsync(string visitTypeId, string departmentId, string providerId, DateTimeOffset startDate, DateTimeOffset endDate);

        /// <summary>
        /// Get imaging schedule dates for providers.
        /// </summary>
        /// <param name="imagingSlot">Visit type identifier.</param>
        /// <returns>Array of <see cref="ScheduleDate"/> with collection of <see cref="TimeSlot"/> inside.</returns>
        public Task<List<ImagingScheduleDate>> GetImagingScheduleAsync(ImagingTimeSlot imagingSlot);
    }
}
