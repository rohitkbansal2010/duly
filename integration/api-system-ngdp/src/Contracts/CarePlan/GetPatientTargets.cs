// <copyright file="GetPatientTargets.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Ngdp.Contracts.CarePlan
{
    public class GetPatientTargets
    {
        [Description("PatientTargetId")]
        public long PatientTargetId { get; set; }
        [Description("TargetId")]
        public long TargetId { get; set; }
        [Description("PatientConditionId")]
        public long PatientConditionId { get; set; }
        [Description("TargetName")]
        public string TargetName { get; set; }
    }
}