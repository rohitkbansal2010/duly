﻿// -----------------------------------------------------------------------
// <copyright file="Chart.Options.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    [Description("Chart Options")]
    public class ChartOptions<TX, TY>
    {
        [Description("Options for chart Scales")]
        public ChartScales<TX, TY> ChartScales { get; set; }
    }
}