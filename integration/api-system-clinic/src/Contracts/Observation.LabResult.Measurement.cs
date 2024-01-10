// <copyright file="Observation.LabResult.Measurement.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Information about the observation measurement.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    public class ObservationLabResultMeasurement
    {
        [Description("The value of the measurement.")]
        public decimal? Value { get; set; }

        [Description("Unit of measurement, if needed.")]
        public string Unit { get; set; }
    }
}