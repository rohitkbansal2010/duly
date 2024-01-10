// <copyright file="GetHealthPlanStats.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    /// <summary>
    /// Condition targets of condition.
    /// </summary>
    public class GetHealthPlanStats
    {
        /// <summary>
        /// Life Goal Count.
        /// </summary>
        public int LifeGoalCount { get; set; }

        /// <summary>
        /// Count of Completed Targets.
        /// </summary>
        public int NumberOfTargetsCompleted { get; set; }

        /// <summary>
        /// Total Number of Targets.
        /// </summary>
        public int TotalNumberOfTargets { get; set; }

        /// <summary>
        /// Count of Completed Actions.
        /// </summary>
        public int NumberOfActionsCompleted { get; set; }

        /// <summary>
        /// Total Number of Actions.
        /// </summary>
        public int TotalNumberOfActions { get; set; }
    }
}