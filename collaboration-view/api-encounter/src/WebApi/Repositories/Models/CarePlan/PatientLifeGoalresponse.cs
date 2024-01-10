// <copyright file="PatientLifeGoalresponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Text.Json.Serialization;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
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

        /// <summary>
        /// Life Goal Description.
        /// </summary>
        public string LifeGoalDescription { get; set; }

        /// <summary>
        /// Life Goal Category Name.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Life Goal Priority.
        /// </summary>
        public int Priority { get; set; }
    }
}
