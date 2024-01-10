// <copyright file="PatientPlanDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class PatientPlanDetails
    {
        [Description("AppointmentId")]
        public string AppointmentId { get; set; }
        [Description("PlanName")]
        public string PlanName { get; set; }
        [Description("PatientPlanId")]
        public long PatientPlanId { get; set; }
        [Description("PatientConditionsDetails")]
        public PatientConditionsDetails PatientConditionsDetails { get; set; }
    }
}