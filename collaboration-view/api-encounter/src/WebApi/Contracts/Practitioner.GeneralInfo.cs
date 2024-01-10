// -----------------------------------------------------------------------
// <copyright file="Practitioner.GeneralInfo.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    [Description("Practitioner general information")]
    public class PractitionerGeneralInfo
    {
        [Description("Practitioner Id")]
        [Required]
        public string Id { get; set; }

        [Description("Name of a Practitioner with parts and usage")]
        [Required]
        public HumanName HumanName { get; set; }

        [Description("Image of a Practitioner")]
        public Attachment Photo { get; set; }

        [Description("Role of a Practitioner")]
        [Required]
        public PractitionerRole Role { get; set; }

        [Description("Speciality of a Practitioner")]
        public List<string> Speciality { get; set; }
    }
}