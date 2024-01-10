// <copyright file="ScheduledAppointment.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("Representation of the scheduled appointment by referral.")]
    public class ScheduledAppointment
    {
        [Description("Identifier by which this appointment is known")]
        public string Id { get; init; }

        [Description("Scheduled appointment date and time")]
        [Required]
        public DateTime DateTime { get; init; }

        [Description("Duration in minutes")]
        [Required]
        public int DurationInMinutes { get; init; }

        [Description("Time zone")]
        public string TimeZone { get; init; }
    }
}