// <copyright file="Observation.LabResult.Component.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Information about the observation component.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    internal class ObservationLabResultComponent
    {
        /// <summary>
        /// The result value if the result is numeric and represents a quantity.
        /// </summary>
        public ObservationLabResultMeasurement[] Measurements { get; set; }

        /// <summary>
        /// Reference Ranges for measurements.
        /// </summary>
        public ObservationLabResultReferenceRange[] ReferenceRange { get; set; }

        /// <summary>
        /// The categorical assessments of the value of the laboratory observation result.
        /// </summary>
        public ObservationLabResultInterpretation[] Interpretations { get; set; }

        /// <summary>
        /// The result value if the result is a text.
        /// </summary>
        public string ValueText { get; set; }

        /// <summary>
        /// Comments about the result.
        /// </summary>
        public string[] Notes { get; set; }
    }
}
