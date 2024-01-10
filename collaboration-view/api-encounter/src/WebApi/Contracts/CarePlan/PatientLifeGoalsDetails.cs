// <copyright file="PatientLifeGoalsDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class PatientLifeGoalsDetails
    {
        [Description("Patient LifeGoal Id")]
        public long PatientLifeGoalId { get; set; }

        [Description("Life Goal Name")]
        public string LifeGoalName { get; set; }
        [Description("Life Goal Description")]
        public string LifeGoalDescription { get; set; }
    }
}
