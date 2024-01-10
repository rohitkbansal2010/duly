// -----------------------------------------------------------------------
// <copyright file="IAppointmentRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Models.Appointment"/> entities.
    /// </summary>
    internal interface IAppointmentRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="Models.Appointment"/> which match with the filter.
        /// </summary>
        /// <param name="siteId">Identifier of clinic.</param>
        /// <param name="startDate">Filter by start date.</param>
        /// <param name="endDate">Filter by end date.</param>
        /// <param name="includedVisitTypes">Visit type Ids that should be included.</param>
        /// <param name="excludedStatuses">Appointment statuses that should be excluded, if null or empty includes all.</param>
        /// <returns>Filtered items of <see cref="Models.Appointment"/>.</returns>
        Task<IEnumerable<Models.Appointment>> GetAppointmentsBySiteIdAndDateRangeAsync(
            string siteId,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            string[] includedVisitTypes,
            Models.AppointmentStatusParam[] excludedStatuses = null);

        /// <summary>
        /// Retrieve all items of <see cref="Models.Appointment"/> which match with the filter.
        /// Where a particular appointment id is only used to find a patient who has that appointment.
        /// </summary>
        /// <param name="appointmentId">Identifier of appointment.</param>
        /// <param name="startDate">Filter by start date.</param>
        /// <param name="endDate">Filter by end date.</param>
        /// <param name="includedStatuses">Appointment statuses that should be included.</param>
        /// <returns>Filtered items of <see cref="Models.Appointment"/>.</returns>
        Task<IEnumerable<Models.Appointment>> GetAppointmentsForSamePatientByAppointmentIdAsync(
            string appointmentId,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            Models.AppointmentStatusParam[] includedStatuses);

        /// <summary>
        /// Retrieve an <see cref="Models.Appointment"/> by id.
        /// </summary>
        /// <param name="appointmentId">Identifier of appointment.</param>
        /// <returns>An instance of <see cref="Models.Appointment"/>.</returns>
        Task<Models.Appointment> GetAppointmentByIdAsync(string appointmentId);
    }
}
