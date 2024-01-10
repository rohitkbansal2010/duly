// -----------------------------------------------------------------------
// <copyright file="Chart.Scales.cs" company="Duly Health and Care">
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
    [Description("Chart Scaling Options")]
    public class ChartScales<TX, TY>
    {
        [Description("Scaling Options for Y Axis")]
        public Axis<TY> YAxis { get; set; }

        [Description("Scaling Options for X Axis")]
        public Axis<TX> XAxis { get; set; }
    }
}