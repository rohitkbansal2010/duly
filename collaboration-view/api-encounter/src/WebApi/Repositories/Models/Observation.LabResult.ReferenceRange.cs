// <copyright file="Observation.LabResult.ReferenceRange.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Reference range for observation.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    internal class ObservationLabResultReferenceRange
    {
        /// <summary>
        /// The highest measurement in Range.
        /// </summary>
        public ObservationLabResultMeasurement High { get; set; }

        /// <summary>
        /// The lowest measurement in Range.
        /// </summary>
        public ObservationLabResultMeasurement Low { get; set; }

        /// <summary>
        /// Description of range.
        /// </summary>
        public string Text { get; set; }
    }
}
