// <copyright file="GetPatientConditionByPatientPlanId.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class GetPatientConditionByPatientPlanId
    {
        [Description("PatientConditionId")]
        public long PatientConditionId { get; set; }
        [Description("ConditionId")]
        public long ConditionId { get; set; }
        [Description("ConditionShortName")]
        public string ConditionShortName { get; set; }
        [Description("ConditionDisplayName")]
        public string ConditionDisplayName { get; set; }
    }
}