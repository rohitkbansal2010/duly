// <copyright file="Party.MemberType.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Member Type")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Contract")]
    public enum MemberType
    {
        [Description("Doctor")]
        Doctor,

        [Description("Caregiver (family member: daughters, son, husband)")]
        Caregiver,

        [Description("Patient")]
        Patient,

        [Description("Other members")]
        Another,

        [Description("Could not identify the type")]
        Unknown
    }
}