// -----------------------------------------------------------------------
// <copyright file="TestReport.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Swashbuckle.AspNetCore.Filters;
using System;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Examples")]
    public class TestReportExampleProvider : IExamplesProvider<TestReport>
    {
        public TestReport GetExamples()
        {
            return BuildTestReport(
                Guid.NewGuid().ToString(),
                "Comprehensive Metabolic Panel",
                DateTime.Today,
                true);
        }

        public TestReport BuildTestReport(
            string id,
            string title,
            DateTime date,
            bool hasAbnormalResults)
        {
            return new TestReport
            {
                Id = id,
                Title = title,
                Date = date,
                HasAbnormalResults = hasAbnormalResults
            };
        }
    }
}