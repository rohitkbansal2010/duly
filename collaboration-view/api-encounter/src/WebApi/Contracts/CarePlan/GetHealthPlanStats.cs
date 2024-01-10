// <copyright file="GetHealthPlanStats.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class GetHealthPlanStats
    {
        [Description("Life Goal Count")]
        public int LifeGoalCount { get; set; }

        [Description("Number of Targets Completed")]
        public int NumberOfTargetsCompleted { get; set; }

        [Description("Total Number Of Targets")]
        public int TotalNumberOfTargets { get; set; }

        [Description("Number of Actions Completed")]
        public int NumberOfActionsCompleted { get; set; }

        [Description("Total Number of Actions")]
        public int TotalNumberOfActions { get; set; }
    }
}