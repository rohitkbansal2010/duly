// <copyright file="UpdatePatientTargets.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class UpdatePatientTargets
    {
        [Description("Patient Plan Idenitifer")]
        public long PatientPlanId { get; set; }

        [Description("Condition Identifier")]
        public IEnumerable<long> ConditionIds { get; set; }

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

        [Description("Measurement Unit")]
        public string MeasurementUnit { get; set; }
    }
}
