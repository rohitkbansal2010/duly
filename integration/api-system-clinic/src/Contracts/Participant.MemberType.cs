// <copyright file="Participant.MemberType.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Persons type in the encounter other than the patient")]
    public enum MemberType
    {
        [Description("Practitioner")]
        Practitioner,

        [Description("A person that is related to a patient, but who is not a direct target of care")]
        RelatedPerson
    }
}