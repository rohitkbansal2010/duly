// <copyright file="CareTeam.Participant.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contracts")]
    [Description("Member of the team")]
    public class CareTeamParticipant
    {
        [Description("Type of involvement")]
        [Required]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "Requirements")]
        public MemberRole MemberRole
        {
            get
            {
                return Member switch
                {
                    PractitionerInCareTeam => MemberRole.Practitioner,
                    _ => throw new ArgumentOutOfRangeException($"Person type ['{Member.GetType()}'] is not supported")
                };
            }
        }

        [Description("Who is involved")]
        [Required]
        public CareTeamMember Member { get; set; }
    }
}