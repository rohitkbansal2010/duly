// <copyright file="TimeSlots.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Describes time slots availability for the specific date.")]
    public class TimeSlots
    {
        [Description("Time slot start time.")]
        [Required]
        public TimeSpan Time { get; init; }

        [Description("Time slot start time that should be shown to user.")]
        public TimeSpan DisplayTime { get; init; }
    }
}