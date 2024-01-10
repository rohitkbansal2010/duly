// <copyright file="ScheduledAppointment.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut
{
    /// <summary>
    /// Representation of the scheduled appointment by referral.
    /// </summary>
    public class ScheduledAppointment
    {
        /// <summary>
        /// Identifier by which this appointment is known.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Scheduled appointment date and time.
        /// </summary>
        public DateTime DateTime { get; init; }

        /// <summary>
        /// Duration in minutes.
        /// </summary>
        public int DurationInMinutes { get; init; }

        /// <summary>
        /// Time zone.
        /// </summary>
        public string TimeZone { get; init; }

        /// <summary>
        /// When the appointment has been scheduled. date and time.
        /// </summary>
        public DateTime ScheduledTime { get; set; }
    }
}
