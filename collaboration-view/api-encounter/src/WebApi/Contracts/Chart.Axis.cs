// -----------------------------------------------------------------------
// <copyright file="Chart.Axis.cs" company="Duly Health and Care">
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
    [Description("Max Value on Axis")]
    public class Axis<TAxis>
    {
        [Description("Max Value on Axis")]
        public TAxis Max { get; set; }

        [Description("Min Value on Axis")]
        public TAxis Min { get; set; }

        [Description("Steps size for scale on Axis")]
        public TAxis StepSize { get; set; }
    }
}