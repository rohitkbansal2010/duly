// <copyright file="Appointment.Status.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.Ngdp.Adapter.Adapters.Models
{
    /// <summary>
    /// Appointment statuses.
    /// </summary>
    public enum AppointmentStatus
    {
        /// <summary>
        /// The patient/patients has/have arrived and is/are waiting to be seen.
        /// </summary>
        Arrived,

        /// <summary>
        /// The appointment has been cancelled.
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
        /// The appointment has been scheduled.
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