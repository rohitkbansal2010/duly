// <copyright file="PatientLifeGoals.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.Ngdp.Contracts.CarePlan
{
    public class PatientLifeGoals
    {
        [Description("Patient LifeGoal Id")]
        public long PatientLifeGoalId { get; set; }

        [Description("Life Goal Name")]
        public string LifeGoalName { get; set; }
        [Description("Life Goal Description")]
        public string LifeGoalDescription { get; set; }
        [Description("Patient Plan Id")]
        public long PatientPlanId { get; set; }
    }
}
