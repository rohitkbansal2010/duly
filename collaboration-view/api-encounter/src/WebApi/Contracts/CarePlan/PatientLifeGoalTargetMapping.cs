// <copyright file="PatientLifeGoalTargetMapping.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class PatientLifeGoalTargetMapping
    {
        [Description("Patient Life Goal Id")]
        public long PatientLifeGoalId { get; set; }
        [Description("Patient Target Id")]
        public long PatientTargetId { get; set; }
    }
}
