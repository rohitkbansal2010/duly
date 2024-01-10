// <copyright file="Slot.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Represents available slot with different timespans")]
    public class Slot
    {
        [Description("Time")]
        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

        [Description("Display Time")]
        [DataType(DataType.Time)]
        public TimeSpan DisplayTime { get; set; }

        [Description("Arrival Time")]
        [DataType(DataType.Time)]
        public TimeSpan ArrivalTime { get; set; }
    }
}
