// <copyright file="ScheduledProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Scheduled provider")]
    public class ScheduledProvider
    {
        [Description("Provider display name")]
        public string DisplayName { get; set; }

        [Description("Provider identifiers")]
        [Required]
        public string[] Identifiers { get; set; }
    }
}