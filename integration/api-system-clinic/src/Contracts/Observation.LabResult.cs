// <copyright file="Observation.LabResult.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Security.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Information about the Lab result observation.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    public class ObservationLabResult
    {
        [Description("Observation Id")]
        [Required]
        [Identity]
        public string Id { get; set; }

        [Description("Component Name")]
        public string ComponentName { get; set; }

        [Description("Observation components.")]
        [Required]
        [MinLength(1)]
        public ObservationLabResultComponent[] Components { get; set; }

        [Description("Date of the observation.")]
        [Required]
        public DateTimeOffset Date { get; set; }
    }
}