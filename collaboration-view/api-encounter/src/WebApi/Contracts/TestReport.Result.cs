// -----------------------------------------------------------------------
// <copyright file="TestReport.Result.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Information about the test report result.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    public class TestReportResult
    {
        [Description("An identifier for the test report result.")]
        [Required]
        public string Id { get; set; }

        [Description("Component Name")]
        [Required]
        public string ComponentName { get; set; }

        [Description("Whether there is abnormal result.")]
        [Required]
        public bool IsAbnormalResult { get; set; }

        [Description("The result value if the result is numeric and represents a quantity.")]
        public TestReportResultMeasurement Measurement { get; set; }

        [Description("Reference ranges for the mesurement")]
        public TestReportResultReferenceRange ReferenceRange { get; set; }

        [Description("The result value if the result is a text.")]
        public string ValueText { get; set; }

        [Description("Comments about the result.")]
        public string[] Notes { get; set; }

        [Description("A categorical assessments, providing a rough qualitative interpretation of the observation value")]
        public string[] Interpretations { get; set; }
    }
}
