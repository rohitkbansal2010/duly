// <copyright file="GetPatientPlanByPatientId.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class GetPatientPlanByPatientId
    {
        [Description("PlanName")]
        public string PlanName { get; set; }
        [Description("PatientPlanId")]
        public long PatientPlanId { get; set; }
        [Description("Flourish Statement")]
        public string FlourishStatement { get; set; }
        [Description("Is Completed")]
        public bool IsCompleted { get; set; }
    }
}