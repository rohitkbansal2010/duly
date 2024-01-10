// <copyright file="PatientLifeGoal.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models.CarePlan
{
    /// <summary>
    /// Life Goal.
    /// </summary>
    public class PatientLifeGoal
    {
        /// <summary>
        /// Life Goal Name.
        /// </summary>
        public string LifeGoalName { get; set; }

        /// <summary>
        /// Life Goal Description.
        /// </summary>
        public string LifeGoalDescription { get; set; }

        /// <summary>
        /// Patient Plan Id.
        /// </summary>
        public long PatientPlanId { get; set; }
    }
}
