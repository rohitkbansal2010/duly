// -----------------------------------------------------------------------
// <copyright file="TestReport.WithResults.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Information about the test report with results.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    public class TestReportWithResults
    {
        [Description("An identifier for the test report.")]
        [Required]
        public string Id { get; set; }

        [Description("Name of the test report.")]
        [Required]
        public string Title { get; set; }

        [Description("Status of test report.")]
        [Required]
        public TestReportStatus Status { get; set; }

        [Description("Effective time of the report.")]
        [Required]
        public DateTimeOffset EffectiveDate { get; set; }

        [Description("Practitioners who performed this report.")]
        public PractitionerGeneralInfo[] Performers { get; set; }

        [Description("Time when the report was issued.")]
        public DateTimeOffset? Issued { get; set; }

        [Description("Results from the report.")]
        public TestReportResult[] Results { get; set; }
    }
}
