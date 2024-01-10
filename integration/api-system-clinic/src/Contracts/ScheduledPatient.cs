// <copyright file="ScheduledPatient.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Scheduled patient")]
    public class ScheduledPatient
    {
        [Description("Patient name")]
        public string Name { get; set; }

        [Description("Patient identifiers")]
        [Required]
        public string[] Identifiers { get; set; }
    }
}