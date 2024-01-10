// <copyright file="TimeSlot.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Represents the time slot with start and end period")]
    public class TimeSlot
    {
        [Description("When action is to take place")]
        [Required]
        public DateTimeOffset StartTime { get; set; }

        [Description("When action is to conclude")]
        [Required]
        public DateTimeOffset EndTime { get; set; }
    }
}