// <copyright file="Encounter.Status.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("The status of an encounter.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Enum in contracts")]
    public enum EncounterStatus
    {
        [Description("The Encounter has not yet started.")]
        Planned,
        [Description("The Patient is present for the encounter, however is not currently meeting with a practitioner.")]
        Arrived,
        [Description("The Encounter has ended before it has begun.")]
        Cancelled,
        [Description("The Encounter has begun and the patient is present / the practitioner and the patient are meeting.")]
        InProgress,
        [Description("The Encounter has ended.")]
        Finished
    }
}
