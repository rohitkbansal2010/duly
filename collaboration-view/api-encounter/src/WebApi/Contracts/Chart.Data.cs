// -----------------------------------------------------------------------
// <copyright file="Chart.Data.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    [Description("Data of the graphic")]
    public class ChartData<TX, TY>
    {
        [Required]
        [Description("Chart data values")]
        public ChartDataValue<TX, TY>[] Values { get; set; }

        [Description("Dimension of the values")]
        public string Dimension { get; set; }
    }
}