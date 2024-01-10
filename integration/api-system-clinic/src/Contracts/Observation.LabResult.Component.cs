// <copyright file="Observation.LabResult.Component.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Information about the observation сomponent.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    public class ObservationLabResultComponent
    {
        [Description("The result value if the result is numeric and represents a quantity.")]
        public ObservationLabResultMeasurement[] Measurements { get; set; }

        [Description("Reference Ranges for mesurements")]
        public ObservationLabResultReferenceRange[] ReferenceRange { get; set; }

        [Description("The categorical assessments of the value of the laboratory observation result.")]
        public ObservationLabResultInterpretation[] Interpretations { get; set; }

        [Description("The result value if the result is a text.")]
        public string ValueText { get; set; }

        [Description("Comments about the result.")]
        public string[] Notes { get; set; }
    }
}