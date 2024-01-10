// <copyright file="PatientLifeGoalResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.Ngdp.Contracts.CarePlan
{
    public class PatientLifeGoalResponse
    {
        [Description("Patient LifeGoal Id")]
        public long PatientLifeGoalId { get; set; }

        [Description("Life Goal Name")]
        public string LifeGoalName { get; set; }
    }
}
