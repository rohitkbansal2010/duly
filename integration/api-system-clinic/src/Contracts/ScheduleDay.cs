// <copyright file="ScheduleDay.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Represents available slots of the day")]
    public class ScheduleDay
    {
        [Description("Date")]
        public DateTimeOffset Date { get; set; }

        [Description("Slots")]
        public Slot[] Slots { get; set; }
    }
}
