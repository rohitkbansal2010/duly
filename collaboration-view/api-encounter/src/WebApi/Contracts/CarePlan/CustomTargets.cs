// <copyright file="CustomTargets.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class CustomTargets
    {
        [Description("TargetName")]
        public string TargetName { get; set; }
        [Description("Min Value")]
        public string MinValue { get; set; }
        [Description("Max Value")]
        public string MaxValue { get; set; }
        [Description("Range")]
        public string Range { get; set; }
        [Description("UnitOfMeasure")]
        public string UnitOfMeasure { get; set; }
        [Description("Description")]
        public string Description { get; set; }
    }
}