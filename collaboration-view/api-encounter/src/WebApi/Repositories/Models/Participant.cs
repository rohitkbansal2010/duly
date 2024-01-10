// <copyright file="Participant.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Participant involved in the encounter.
    /// </summary>
    internal class Participant
    {
        /// <summary>
        /// Participant's type involved in the encounter other than the patient.
        /// </summary>
        public MemberType MemberType { get; set; }

        /// <summary>
        /// Human specific information about person.
        /// </summary>
        public HumanGeneralInfoWithPhoto Person { get; set; }
    }
}