// <copyright file="ConditionCategory.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Enum in Contacts")]
    [Description("Condition category")]
    public enum ConditionCategory
    {
        [Description("Problems patient has")]
        Problem
    }
}