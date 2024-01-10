// <copyright file="GetPatientPlan.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.Ngdp.Contracts.CarePlan
{
    public class GetPatientPlan
    {
        [Description("PlanName")]
        public string PlanName { get; set; }
        [Description("PatientPlanId")]
        public long PatientPlanId { get; set; }
        [Description("Flourish Statement")]
        public string FlourishStatement { get; set; }
    }
}