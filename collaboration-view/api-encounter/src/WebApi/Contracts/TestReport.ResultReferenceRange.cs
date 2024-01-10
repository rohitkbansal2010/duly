// -----------------------------------------------------------------------
// <copyright file="TestReport.ResultReferenceRange.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Information about the reference range.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    public class TestReportResultReferenceRange
    {
        [Description("The highest measurement in the reference range.")]
        public TestReportResultMeasurement High { get; set; }

        [Description("The lowest measurement in the reference range.")]
        public TestReportResultMeasurement Low { get; set; }

        [Description("Display version of the reference range.")]
        public string Text { get; set; }
    }
}
