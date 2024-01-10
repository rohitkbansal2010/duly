// <copyright file="PatientLifeGoal.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class PatientLifeGoal
    {
        [Description("Patient LifeGoal Id")]
        public long PatientLifeGoalId { get; set; }

        [Description("Life Goal Name")]
        public string LifeGoalName { get; set; }

        [Description("Life Goal Description")]
        public string LifeGoalDescription { get; set; }

        [Description("Life Goal Category")]
        public string CategoryName { get; set; }

        [Description("Priority of Life Goal")]
        public int Priority { get; set; }
    }
}
