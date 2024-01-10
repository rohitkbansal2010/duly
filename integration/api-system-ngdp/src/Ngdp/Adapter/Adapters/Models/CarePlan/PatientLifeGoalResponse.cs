// <copyright file="PatientLifeGoalResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models.CarePlan
{
    /// <summary>
    /// patient lifegoal response.
    /// </summary>
    public class PatientLifeGoalResponse

    {
        /// <summary>
        /// Patient LifeGoal Id.
        /// </summary>
        public long PatientLifeGoalId { get; set; }

        /// <summary>
        /// LifeGoal Name.
        /// </summary>
        public string LifeGoalName { get; set; }

    }
}
