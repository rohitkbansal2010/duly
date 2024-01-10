// <copyright file="PostRequestForLifeGoals.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    /// <summary>
    /// Patient Life Goals.
    /// </summary>
    public class PostRequestForLifeGoals
    {
        /// <summary>
        /// Patient Plan Id.
        /// </summary>
        public long PatientPlanId { get; set; }

        /// <summary>
        /// Life Goals Detail.
        /// </summary>
        public IEnumerable<PatientLifeGoal> PatientLifeGoal { get; set; }

        /// <summary>
        /// Life Goals to be deleted.
        /// </summary>
        public IEnumerable<long> DeletedLifeGoalIds { get; set; }
    }
}