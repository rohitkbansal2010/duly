// <copyright file="ConditionClinicalStatus.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Status of a Condition")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Enum in contracts")]
    public enum ConditionClinicalStatus
    {
        [Description("The subject is currently experiencing the condition or situation, there is evidence of the condition or situation, or considered to be a significant risk.")]
        Active,
        [Description("The subject is no longer experiencing the condition or situation and there is no longer evidence or appreciable risk of the condition or situation.")]
        Inactive,
        [Description("The subject is not presently experiencing the condition or situation and there is a negligible perceived risk of the condition or situation returning.")]
        Resolved
    }
}