// <copyright file="CareTeam.Practitioner.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Practitioner as a member of care team.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    internal class PractitionerInCareTeam : CareTeamMember
    {
        /// <summary>
        /// Practitioner general information.
        /// </summary>
        public PractitionerGeneralInfo Practitioner { get; set; }
    }
}