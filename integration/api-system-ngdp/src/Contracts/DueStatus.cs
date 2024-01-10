// <copyright file="DueStatus.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Ngdp.Contracts
{
    /// <summary>
    /// Immunization Recommendation Status Codes.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Contract")]
    public enum DueStatus
    {
        [Description("The patient is not due for their next vaccination.")]
        NotDue = 1,

        [Description("The patient is due soon for their next vaccination.")]
        DueSoon,

        [Description("The patient is due for their next vaccination.")]
        DueOn,

        [Description("The patient is considered overdue for their next vaccination.")]
        Overdue,

        [Description("The patient is considered postpone for their next vaccination.")]
        Postponed
    }
}