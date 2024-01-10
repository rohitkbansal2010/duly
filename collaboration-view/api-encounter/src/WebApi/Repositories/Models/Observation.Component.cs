// <copyright file="Observation.Component.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Information about the observation component.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    internal class ObservationComponent
    {
        /// <summary>
        /// Type of observation component measurements. <see cref="ObservationComponentType"/>>
        /// If null, it repeats the Observation type.
        /// </summary>
        public ObservationComponentType? Type { get; set; }

        /// <summary>
        /// Array of <see cref="ObservationMeasurement"/>.
        /// Must exist at least one element.
        /// </summary>
        public ObservationMeasurement[] Measurements { get; set; }
    }
}
