// -----------------------------------------------------------------------
// <copyright file="TestReport.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Represents an information about the test report.")]
    public class TestReport
    {
        [Description("An identifier for the test report.")]
        [Required]
        public string Id { get; set; }

        [Description("Name of the test report.")]
        [Required]
        public string Title { get; set; }

        [Description("Clinically relevant date for report.")]
        [Required]
        public DateTimeOffset Date { get; set; }

        [Description("Whether there are abnormal results in the report.")]
        [Required]
        public bool HasAbnormalResults { get; set; }
    }

    public class MultipleTestReportResults
    {
        [Description("An identifier for the test report.")]
        public string Id { get; set; }

        public TestReportWithResults TestResultCollection { get; set; }
    }
}
