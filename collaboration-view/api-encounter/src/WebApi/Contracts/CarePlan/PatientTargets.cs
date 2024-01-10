// <copyright file="PatientTargets.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class PatientTargets
    {
        [Description("Patient Plan Id")]
        public long PatientPlanId { get; set; }

        [Description("Condition Id")]
        public long ConditionId { get; set; }

        [Description("Target Id")]
        public long TargetId { get; set; }

        [Description("Min value")]
        public string MinValue { get; set; }

        [Description("Max value")]
        public string MaxValue { get; set; }

        [Description("Measurement Unit")]
        public string MeasurementUnit { get; set; }

        [Description("Target value")]
        public string TargetValue { get; set; }

        [Description("Base Value")]
        public string BaseValue { get; set; }

        [Description("Recent Value")]
        public string RecentValue { get; set; }
    }
}