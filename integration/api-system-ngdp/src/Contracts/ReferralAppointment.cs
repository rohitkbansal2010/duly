// <copyright file="ReferralAppointment.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("A result of scheduling of the referral.")]
    public class ReferralAppointment
    {
        [Description("Referral Id")]
        [Required]
        public string ReferralId { get; init; }

        [Description("Date and Time when the referral appointment was scheduled")]
        [Required]
        public DateTime ScheduledTime { get; init; }

        [Description("Original information about appointment")]
        [Required]
        public ScheduledAppointment Appointment { get; init; }

        [Description("Information about the visit")]
        [Required]
        public Visit Visit { get; init; }

        [Description("Information about department of the provider")]
        [Required]
        public Department Department { get; init; }

        [Description("Location where the original procedure takes place")]
        [Required]
        public Location Location { get; init; }

        [Description("Information about the provider")]
        [Required]
        public ScheduledProvider Provider { get; init; }
    }
}