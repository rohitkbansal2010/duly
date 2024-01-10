﻿// <copyright file="GetPatientLifeGoalTargetMapping.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class GetPatientLifeGoalTargetMapping
    {
        [Description("PatientLifeGoalId")]
        public long PatientLifeGoalId { get; set; }

        [Description("LifeGoalName")]

        public string LifeGoalName { get; set; }

        [Description("LifeGoalDescription")]

        public string LifeGoalDescription { get; set; }

        [Description("Category of Life Goal")]
        public string CategoryName { get; set; }

        [Description("Priority of Life Goal")]
        public int Priority { get; set; }
    }
}
