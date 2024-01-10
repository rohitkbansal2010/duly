// -----------------------------------------------------------------------
// <copyright file="Vital.History.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]

    [Description("Historic data for Vital")]
    public class VitalHistory
    {
        [Description("Chart of historic data for set Vital")]
        public Chart<DateTimeOffset, decimal> Chart { get; set; }
    }
}