// -----------------------------------------------------------------------
// <copyright file="Practitioner.Role.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.Serialization;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Practitioner roles")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Contract")]
    public enum PractitionerRole
    {
        [Description("Medical assistent (MA)")]
        [EnumMember(Value = "MA")]
        MedicalAssistant,

        [Description("Primary care physician (PCP)")]
        [EnumMember(Value = "PCP")]
        PrimaryCarePhysician,

        [Description("Could not identify the role")]
        [EnumMember(Value = "Unknown")]
        Unknown
    }
}