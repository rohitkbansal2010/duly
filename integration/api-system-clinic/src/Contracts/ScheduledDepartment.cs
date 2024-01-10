// <copyright file="ScheduledDepartment.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Scheduled department")]
    public class ScheduledDepartment
    {
        [Description("Department name")]
        public string Name { get; set; }

        [Description("Instructions for finding the location")]
        public string[] LocationInstructions { get; set; }

        [Description("Department identifiers")]
        [Required]
        public string[] Identifiers { get; set; }

        [Description("Department address")]
        public Address Address { get; set; }

        [Description("Department medical branch")]
        public Specialty Specialty { get; set; }

        [Description("Department local time zone")]
        public OfficialTimeZone OfficialTimeZone { get; set; }

        [Description("Department contact phones")]
        [Required]
        public Phone[] Phones { get; set; }
    }
}
