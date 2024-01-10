// -----------------------------------------------------------------------
// <copyright file="TestReport.ExamplesProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Examples")]
    public class TestReportExamplesProvider : IExamplesProvider<IEnumerable<TestReport>>
    {
        public IEnumerable<TestReport> GetExamples()
        {
            var provider = new TestReportExampleProvider();

            yield return provider.BuildTestReport(
                Guid.NewGuid().ToString(),
                "Comprehensive Metabolic Panel",
                new DateTime(2021, 8, 14),
                true);

            yield return provider.BuildTestReport(
                Guid.NewGuid().ToString(),
                "Alanine Aminotransferease",
                new DateTime(2021, 3, 19),
                false);

            yield return provider.BuildTestReport(
                Guid.NewGuid().ToString(),
                "Urinalysis",
                new DateTime(2021, 3, 19),
                true);
        }
    }
}