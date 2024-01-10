// <copyright file="Immunization.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    public class Immunization
    {
        [Description("Patient who has the immunization")]
        [Required]
        public Patient Patient { get; set; }

        [Required]
        [Description("The name of the vaccine")]
        public string VaccineName { get; set; }

        [Required]
        [Description("Due date")]
        public DateTimeOffset DueDate { get; set; }

        [Required]
        [Description("Due status")]
        public DueStatus Status { get; set; }
    }
}
