// <copyright file="ConditionTargets.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.Ngdp.Contracts.CarePlan
{
    public class ConditionTargets
    {
        [Description("Target Id")]
        public long TargetId { get; set; }

        [Description("Condition Id")]
        public string ConditionId { get; set; }

        [Description("Target Name")]
        public string TargetName { get; set; }

        [Description("Min Value")]
        public string MinValue { get; set; }

        [Description("Max Value")]
        public string MaxValue { get; set; }

        [Description("Description")]
        public string Description { get; set; }

        [Description("Measurement Value")]
        public string MeasurementValue { get; set; }

        [Description("Measurement Unit")]
        public string MeasurementUnit { get; set; }

        [Description("Active")]
        public bool Active { get; set; }
    }
}