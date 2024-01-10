// <copyright file="CareTeamWithParticipants.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Contracts
{
    /// <summary>
    /// Care Team & its Participants.
    /// </summary>
    public class CareTeamWithParticipants : IResourceWithCompartments<R4.CareTeam>
    {
        /// <summary>
        /// Care Team instance.
        /// </summary>
        public R4.CareTeam Resource { get; set; }

        /// <summary>
        /// Practitioners in the care team.
        /// </summary>
        public PractitionerWithRoles[] Practitioners { get; set; }
    }
}