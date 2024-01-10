// <copyright file="Observation.LabResult.ReferenceRange.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Reference range for observation.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    public class ObservationLabResultReferenceRange
    {
        [Description("The highest measurement in Range.")]
        public ObservationLabResultMeasurement High { get; set; }

        [Description("The lowest measurement in Range.")]
        public ObservationLabResultMeasurement Low { get; set; }

        [Description("Decription of range.")]
        public string Text { get; set; }
    }
}
