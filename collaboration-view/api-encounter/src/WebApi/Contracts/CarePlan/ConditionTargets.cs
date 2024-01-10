// <copyright file="ConditionTargets.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class ConditionTargets
    {
        [Description("Target Id")]
        public long TargetId { get; set; }

        [Description("Condition Id")]
        public string ConditionId { get; set; }

        [Description("Target Name")]
        public string TargetName { get; set; }

        [Description("Minimum Value")]
        public string TargetMinValue { get; set; }

        [Description("Maximum Value")]
        public string TargetMaxValue { get; set; }

        [Description("Category Name")]
        public string CategoryName { get; set; }

        [Description("Description")]
        public string Description { get; set; }

        [Description("Measurement Unit")]
        public string MeasurementUnit { get; set; }

        [Description("Active")]
        public bool Active { get; set; }
    }
}