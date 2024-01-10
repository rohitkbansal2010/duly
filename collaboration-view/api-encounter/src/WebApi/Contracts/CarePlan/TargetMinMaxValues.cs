// <copyright file="TargetMinMaxValues.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class TargetMinMaxValues
    {
        public TargetMinMaxValues(string min, string max)
        {
            TargetMinValue = min;
            TargetMaxValue = max;
        }

        [Description("Min Value")]
        public string TargetMinValue { get; set; }
        [Description("Max Value")]
        public string TargetMaxValue { get; set; }
    }
}