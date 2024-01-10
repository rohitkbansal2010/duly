// <copyright file="PatientTarget.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Ngdp.Contracts.CarePlan
{
    public class PatientTarget
    {
        [Description("Patient Plan Id")]
        public long PatientPlanId { get; set; }

        [Description("Condition Id")]
        public long ConditionId { get; set; }

        [Description("Target Type")]
        public string TargetType { get; set; }

        [Description("Target Id")]
        public long TargetId { get; set; }

        [Description("Custom Target Id")]
        public long CustomTargetId { get; set; }

        [Description("Min value")]
        public string MinValue { get; set; }

        [Description("Max value")]
        public string MaxValue { get; set; }

        [Description("Measurement Value")]
        public string MeasurementValue { get; set; }

        [Description("Measurement Unit")]
        public string MeasurementUnit { get; set; }

        [Description("Created By")]
        public string CreatedBy { get; set; }

        [Description("Updated By")]
        public string UpdatedBy { get; set; }
    }
}