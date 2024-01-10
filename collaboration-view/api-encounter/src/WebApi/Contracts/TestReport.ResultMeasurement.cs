// -----------------------------------------------------------------------
// <copyright file="TestReport.ResultMeasurement.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Information about the test report result measurement.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    public class TestReportResultMeasurement
    {
        [Description("The value of the measurement.")]
        public decimal? Value { get; set; }

        [Description("Unit of measurement, if needed.")]
        public string Unit { get; set; }
    }
}
