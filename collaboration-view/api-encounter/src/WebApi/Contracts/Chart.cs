// -----------------------------------------------------------------------
// <copyright file="Chart.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("A graphical representation for data visualization")]
    public class Chart<TX, TY>
    {
        [Description("Data sets of the chart. It's possible to display more than one graphic on the chart")]
        [Required]
        public ChartDataset<TX, TY>[] Datasets { get; set; }

        [Description("Chart options")]
        public ChartOptions<TX, TY> ChartOptions { get; set; }
    }
}