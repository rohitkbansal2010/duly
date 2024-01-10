// -----------------------------------------------------------------------
// <copyright file="Allergy.Reaction.Severity.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("The degree of the patient's reaction.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1602:Enumeration items should be documented",
        Justification = "Contract")]
    public enum AllergyReactionSeverity
    {
        [Description("Causes mild physiological effects.")]
        Mild,

        [Description("Causes moderate physiological effects.")]
        Moderate,

        [Description("Causes severe physiological effects.")]
        Severe
    }
}