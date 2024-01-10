// <copyright file="CareTeam.Practitioner.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contracts")]
    [Description("Practitioner as a member of care team.")]
    public class PractitionerInCareTeam : CareTeamMember
    {
        [Required]
        [Description("Practitioner general information.")]
        public PractitionerGeneralInfo Practitioner { get; set; }
    }
}