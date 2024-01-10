// <copyright file="Immunization.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Security.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Record of patient's immunization, including vaccine and vaccine administration details.")]
    public class Immunization
    {
        [Description("Business identifier for this immunization")]
        [Required]
        [Identity]
        public string Id { get; set; }

        [Description("Immunization status")]
        [Required]
        public ImmunizationStatus Status { get; set; }

        [Description("Administered vaccine")]
        [Required]
        public Vaccine Vaccine { get; set; }

        [Description("Vaccination administration date and time")]
        public DateTimeOffset? OccurrenceDateTime { get; set; }

        [Description("Information about the amount of vaccine administered")]
        public Quantity Dose { get; set; }

        [Description("Vaccination notes")]
        public string[] Notes { get; set; }

        [Description("Why immunization did not occur")]
        public ImmunizationStatusReason StatusReason { get; set; }
    }
}
