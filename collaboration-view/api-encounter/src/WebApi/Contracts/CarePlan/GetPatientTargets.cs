// <copyright file="GetPatientTargets.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class GetPatientTargets
    {
        [Description("Patient Target Identifier")]
        public long PatientTargetId { get; set; }

        [Description("Target Idenitifer")]
        public long TargetId { get; set; }

        [Description("Target Name")]
        public string TargetName { get; set; }

        [Description("Minimum Value")]
        public string MinValue { get; set; }

        [Description("Maximum Value")]
        public string MaxValue { get; set; }

        [Description("Base Value")]
        public string BaseValue { get; set; }

        [Description("Target Value")]
        public string TargetValue { get; set; }

        [Description("Recent Value")]
        public string RecentValue { get; set; }

        [Description("Normal Value")]
        public string NormalValue { get; set; }

        [Description("Measurement Unit")]
        public string MeasurementUnit { get; set; }

        [Description("Patient Condition Identifier")]
        public string PatientConditionId { get; set; }

        [Description("Condition Identifier")]
        public long ConditionId { get; set; }

        [Description("Review Status")]
        public int IsReviewed { get; set; }
    }
}