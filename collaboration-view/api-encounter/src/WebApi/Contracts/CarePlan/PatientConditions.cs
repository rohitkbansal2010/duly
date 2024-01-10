// <copyright file="PatientConditions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class PatientConditions
    {
        [Description("PatiendPlanId")]
        public long PatientPlanId { get; set; }
        [Description("Condition Ids to be added")]
        public long[] AddConditionIds { get; set; }
        [Description("Condition Ids to be removed")]
        public long[] RemoveConditionIds { get; set; }
    }
}