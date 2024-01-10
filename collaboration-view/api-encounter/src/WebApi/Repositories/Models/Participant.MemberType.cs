// <copyright file="Participant.MemberType.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Member Type.
    /// </summary>
    public enum MemberType
    {
        /// <summary>
        /// Practitioner.
        /// </summary>
        Practitioner,

        /// <summary>
        /// A person that is related to a patient, but who is not a direct target of care.
        /// </summary>
        RelatedPerson
    }
}