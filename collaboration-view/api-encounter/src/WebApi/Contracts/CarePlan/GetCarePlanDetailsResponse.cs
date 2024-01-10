// <copyright file="GetCarePlanDetailsResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class GetCarePlanDetailsResponse
    {
        [Description("PatientPlanDetails")]
        public PatientPlanDetails PatientPlanDetails { get; set; }
        [Description("PatientLifeGoalsDetails")]
        public PatientLifeGoalsDetails PatientLifeGoalsDetails { get; set; }
    }
}