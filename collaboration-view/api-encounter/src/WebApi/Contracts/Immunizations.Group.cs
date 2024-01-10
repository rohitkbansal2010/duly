// -----------------------------------------------------------------------
// <copyright file="Immunizations.Group.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Represents an information about the patient's past immunization group.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    public class ImmunizationsGroup
    {
        [Description("The title of the immunization group.")]
        [Required]
        public string Title { get; set; }

        [Description("Vaccinations of the immunization group.")]
        [Required]
        public Vaccination[] Vaccinations { get; set; }
    }
}
