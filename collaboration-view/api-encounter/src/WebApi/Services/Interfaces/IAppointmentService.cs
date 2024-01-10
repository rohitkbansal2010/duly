// -----------------------------------------------------------------------
// <copyright file="IAppointmentService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Encounter.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// Service for working on <see cref="Appointment"/> entities.
    /// </summary>
    public interface IAppointmentService
    {
        /// <summary>
        /// Retrieve all items of <see cref="Appointment"/> which match with the filter.
        /// </summary>
        /// <param name="siteId">Identifier of clinic.</param>
        /// <param name="startDate">Filter by start date.</param>
        /// <param name="endDate">Filter by end date.</param>
        /// <returns>Filtered items of <see cref="Appointment"/>.</returns>
        Task<IEnumerable<Appointment>> GetAppointmentsBySiteIdAndDateRangeAsync(string siteId, DateTimeOffset startDate, DateTimeOffset endDate);

        /// <summary>
        /// Returns <see cref="PatientAppointments"/> that represents an information about a grouped schedule appointments
        /// for the patient which has an appointment with specific appointment Id.
        /// </summary>
        /// <param name="appointmentId">Identifier of appointment.</param>
        /// <returns>An instance of <see cref="PatientAppointments"/>.</returns>
        Task<PatientAppointments> GetAppointmentsForSamePatientByAppointmentIdAsync(string appointmentId);

        /// <summary>
        /// Returns <see cref="PractitionerGeneralInfo"/> who participates in appointment with specific appointmentId.
        /// </summary>
        /// <param name="appointmentId">Identifier of appointment.</param>
        /// <returns>An instance of <see cref="PractitionerGeneralInfo"/>.</returns>
        Task<PractitionerGeneralInfo> GetPractitionerByAppointmentIdAsync(string appointmentId);
    }
}
