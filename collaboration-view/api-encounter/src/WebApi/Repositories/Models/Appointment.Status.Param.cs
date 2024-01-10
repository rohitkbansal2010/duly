// -----------------------------------------------------------------------
// <copyright file="Appointment.Status.Param.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// The status of an appointment to be used for queries.
    /// </summary>
    internal enum AppointmentStatusParam
    {
        /// <summary>
        /// The patient is present for the appointment, however is not currently meeting with a practitioner.
        /// </summary>
        Arrived,

        /// <summary>
        /// The appointment has ended before it has begun.
        /// </summary>
        Canceled,

        /// <summary>
        /// The appointment has ended.
        /// </summary>
        Completed,

        /// <summary>
        /// No additional description is available.
        /// </summary>
        LeftWithoutSeen,

        /// <summary>
        /// The patient has not arrived for the appointment.
        /// </summary>
        NoShow,

        /// <summary>
        /// The appointment is scheduled.
        /// </summary>
        Scheduled,

        /// <summary>
        /// The appointment status is unresolved in the datasource.
        /// </summary>
        Unresolved,

        /// <summary>
        /// No additional description is available.
        /// </summary>
        ChargeEntered
    }
}
