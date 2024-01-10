// <copyright file="Observation.Measurement.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Information about the observation measurement.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    internal class ObservationMeasurement
    {
        /// <summary>
        /// The value of the measurement.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Unit of measurement, if needed.
        /// </summary>
        public string Unit { get; set; }
    }
}
