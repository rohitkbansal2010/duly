// -----------------------------------------------------------------------
// <copyright file="Chart.Dataset.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    [Description("Data set")]
    public class ChartDataset<TX, TY>
    {
        [Description("Legend of the graphic")]
        public string Label { get; set; }

        [Description("Data with descriptions")]
        [Required]
        public ChartData<TX, TY> Data { get; set; }

        [Description("Ranges for the dataset")]
        public KeyValuePair<string, ExpectedRange<TY>>[] Ranges { get; set; }

        [Description("Set true if this data set is intended for display.")]
        public bool Visible { get; set; }
    }
}