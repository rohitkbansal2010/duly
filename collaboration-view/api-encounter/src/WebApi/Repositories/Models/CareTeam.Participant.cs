// <copyright file="CareTeam.Participant.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Member of the team.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    internal class CareTeamParticipant
    {
        /// <summary>
        /// Type of involvement.
        /// </summary>
        public MemberRole MemberRole { get; set; }

        /// <summary>
        /// Who is involved.
        /// </summary>
        public CareTeamMember Member { get; set; }
    }
}