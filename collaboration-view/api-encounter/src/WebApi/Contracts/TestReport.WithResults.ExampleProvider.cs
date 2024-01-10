// -----------------------------------------------------------------------
// <copyright file="TestReport.WithResults.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Examples")]
    public class TestReportWithResultsExampleProvider : IExamplesProvider<TestReportWithResults>
    {
        public TestReportWithResults GetExamples()
        {
            var effectiveDate = new DateTime(2021, 8, 14, 10, 17, 31, DateTimeKind.Utc);

            return new TestReportWithResults
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Comprehensive Metabolic Panel",
                Status = TestReportStatus.Final,
                EffectiveDate = effectiveDate,
                Issued = effectiveDate.AddHours(3),
                Performers = BuildPerformers(),
                Results = BuildResults().ToArray()
            };
        }

        private static PractitionerGeneralInfo[] BuildPerformers()
        {
            var practitionerGeneralInfoExampleProvider = new PractitionerGeneralInfoExampleProvider();
            return new[]
            {
                practitionerGeneralInfoExampleProvider.GetExamples()
            };
        }

        private static IEnumerable<TestReportResult> BuildResults()
        {
            yield return new TestReportResult
            {
                Id = Guid.NewGuid().ToString(),
                ComponentName = "Cholesterol, Total",
                Measurement = new TestReportResultMeasurement
                {
                    Value = 150M,
                    Unit = "mg/dL"
                },
                ReferenceRange = new TestReportResultReferenceRange
                {
                    Text = "<200"
                },
                Notes = new[]
                {
                    @"Desirable  <200 mg/dL\r\nBorderline  200-239 mg/dL\r\nHigh      >=240 mg/dL\r\n\r\n"
                }
            };

            yield return new TestReportResult
            {
                Id = Guid.NewGuid().ToString(),
                ComponentName = "LDL Cholesterol",
                Measurement = new TestReportResultMeasurement
                {
                    Value = 117M,
                    Unit = "mg/dL"
                },
                ReferenceRange = new TestReportResultReferenceRange
                {
                    Text = "<100"
                },
                Notes = new[]
                {
                    @"Desirable <100 mg/dL \r\nBorderline 100-129 mg/dL \r\nHigh     >=130mg/dL\r\n\r\n"
                },
                IsAbnormalResult = true
            };

            yield return new TestReportResult
            {
                Id = Guid.NewGuid().ToString(),
                ComponentName = "Sodium",
                Measurement = new TestReportResultMeasurement
                {
                    Value = 138M,
                    Unit = "mmol/L"
                },
                ReferenceRange = new TestReportResultReferenceRange
                {
                    Low = new TestReportResultMeasurement
                    {
                        Value = 136M,
                        Unit = "mmol/L"
                    },
                    High = new TestReportResultMeasurement
                    {
                        Value = 145M,
                        Unit = "mmol/L"
                    },
                    Text = "136 - 145 mmol/L"
                }
            };
        }
    }
}