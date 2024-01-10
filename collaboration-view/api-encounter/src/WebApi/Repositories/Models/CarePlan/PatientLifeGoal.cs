// <copyright file="PatientLifeGoal.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Text.Json.Serialization;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    /// <summary>
    /// Patient Life Goal.
    /// </summary>
    public class PatientLifeGoal
    {
        /// <summary>
        /// Patient LifeGoal Id.
        /// </summary>
        public long PatientLifeGoalId { get; set; }

        /// <summary>
        /// Name of Life Goal of Patient.
        /// </summary>
        public string LifeGoalName { get; set; }

        /// <summary>
        /// Description of Life Goal of Patient.
        /// </summary>
        public string LifeGoalDescription { get; set; }

        /// <summary>
        /// Category.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Priority.
        /// </summary>
        public int Priority { get; set; }
    }
}
