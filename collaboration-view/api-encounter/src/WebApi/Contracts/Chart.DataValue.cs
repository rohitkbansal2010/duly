// <copyright file="Chart.DataValue.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    [Description("One item of the graphic")]
    public class ChartDataValue<TX, TY>
    {
        [Required]
        [Description("Value on X axis")]
        public TX X { get; set; }

        [Required]
        [Description("Value on Y axis")]
        public TY Y { get; set; }
    }
}