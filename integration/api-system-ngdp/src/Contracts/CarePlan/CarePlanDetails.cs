// <copyright file="CarePlanDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.Ngdp.Contracts.CarePlan
{
    public class CarePlanDetails
    {
        [Description("PatientPlanDetails")]
        public PatientPlanDetails PatientPlanDetails { get; set; }
        [Description("PatientLifeGoalsDetails")]
        public PatientLifeGoalsDetails PatientLifeGoalsDetails { get; set; }
    }
}