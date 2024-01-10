// <copyright file="Appointment.Visit.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("A visit which cpecifies the appointment")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    public class AppointmentVisit
    {
        [Description("Visit type display name such as \"8014\"")]
        [Required]
        public string TypeId { get; set; }

        [Description("Visit type display name such as \"NEW PT -SPECIALTY CONSULT\"")]
        [Required]
        public string Type { get; set; }

        [Description("Visit type display name such as \"New Patient Office Visit\"")]
        public string TypeDisplayName { get; set; }
    }
}
