// <copyright file="TimeSlot.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts.Interfaces;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Represents the time slot with start and end period")]
    public class TimeSlot : IDulyResource
    {
        [Description("When action is to take place")]
        [Required]
        public DateTimeOffset StartTime { get; set; }

        [Description("When action is to conclude")]
        [Required]
        public DateTimeOffset EndTime { get; set; }
    }
}
