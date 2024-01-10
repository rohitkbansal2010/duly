// <copyright file="Observation.Measurement.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Information about the observation measurement.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    public class ObservationMeasurement
    {
        [Description("The value of the measurement.")]
        [Required]
        public decimal Value { get; set; }

        [Description("Unit of measurement, if needed.")]
        public string Unit { get; set; }
    }
}
