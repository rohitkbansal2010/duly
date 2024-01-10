// -----------------------------------------------------------------------
// <copyright file="Vital.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Information about the measurements of the vital.")]
    public class Vital
    {
        [Description("Type of the vital.")]
        [Required]
        public VitalType VitalType { get; set; }

        [Description("Information about of the each the vital measurement.")]
        [Required]
        public VitalMeasurement[] Measurements { get; set; }
    }
}
