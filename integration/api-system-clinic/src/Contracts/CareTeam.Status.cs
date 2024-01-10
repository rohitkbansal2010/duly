// <copyright file="CareTeam.Status.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Contract")]
    [Description("Indicates the status of the care team.")]
    public enum CareTeamStatus
    {
        [Description("The care team is currently participating in the coordination and delivery of care.")]
        Active
    }
}