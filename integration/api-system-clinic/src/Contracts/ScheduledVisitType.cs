// <copyright file="ScheduledVisitType.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Scheduled visit type")]
    public class ScheduledVisitType
    {
        [Description("Name")]
        public string Name { get; set; }

        [Description("Visit type display name")]
        public string DisplayName { get; set; }

        [Description("Patient instructions")]
        [Required]
        public string[] PatientInstructions { get; set; }

        [Description("Visit type identifiers")]
        [Required]
        public string[] Identifiers { get; set; }
    }
}