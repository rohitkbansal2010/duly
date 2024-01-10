// <copyright file="Observation.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Information about the observation.")]
    public class Observation
    {
        [Description("Type of the observation.")]
        [Required]
        public ObservationType Type { get; set; }

        [Description("Observation components.")]
        [Required]
        [MinLength(1)]
        public ObservationComponent[] Components { get; set; }

        [Description("Date of the observation.")]
        [Required]
        public DateTimeOffset Date { get; set; }
    }
}
