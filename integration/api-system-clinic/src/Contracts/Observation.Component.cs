// <copyright file="Observation.Component.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Information about the observation сomponent.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    public class ObservationComponent
    {
        [Description("Measurements of observation component")]
        [Required]
        [MinLength(1)]
        public ObservationMeasurement[] Measurements { get; set; }

        [Description("Type of observation component measurements")]
        public ObservationComponentType? Type { get; set; }
    }
}
