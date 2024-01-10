// <copyright file="Condition.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Security.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    public class Condition
    {
        [Description("Condition Id")]
        [Required]
        [Identity]
        public string Id { get; set; }

        [Description("Name of a condition")]
        public string Name { get; set; }

        [Description("Status of Condition")]
        [Required]
        public ConditionClinicalStatus ClinicalStatus { get; set; }

        [Description("Date condition was recorded")]
        public DateTimeOffset RecordedDate { get; set; }

        [Description("Date condition was resolved")]
        public Period AbatementPeriod { get; set; }
    }
}