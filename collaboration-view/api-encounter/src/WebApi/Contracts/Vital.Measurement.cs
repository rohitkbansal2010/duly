// -----------------------------------------------------------------------
// <copyright file="Vital.Measurement.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Information about the vital measurement.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    public class VitalMeasurement
    {
        [Description("Type of the vital measurements.")]
        [Required]
        public VitalMeasurementType MeasurementType { get; set; }

        [Description("The value of the measurement.")]
        [Required]
        public decimal Value { get; set; }

        [Description("The time of the measurement.")]
        [Required]
        public DateTimeOffset Measured { get; set; }

        [Description("Unit of measurement, if needed.")]
        public string Unit { get; set; }

        [Description("The maximum value of the scale, if relevant to measurement.")]
        public decimal? MaxScaleValue { get; set; }
    }
}
