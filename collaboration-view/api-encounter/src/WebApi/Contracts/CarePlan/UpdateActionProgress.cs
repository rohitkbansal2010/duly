// <copyright file="UpdateActionProgress.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class UpdateActionProgress
    {
        [Description("Progress")]
        public int Progress { get; set; }
        [Description("Notes")]
        public string Notes { get; set; }
    }
}
