// <copyright file="GetPatientPlanByPatientIdModel.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    public class GetPatientPlanByPatientIdModel
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