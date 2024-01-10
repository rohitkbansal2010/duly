// <copyright file="CareTeam.Category.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Contract")]
    [Description("Indicates the type of care team.")]
    public enum CareTeamCategory
    {
        [Description("Longitudinal care-coordination focused care team")]
        Longitudinal,

        [Description("Episode of care-focused care team")]
        Episode
    }
}