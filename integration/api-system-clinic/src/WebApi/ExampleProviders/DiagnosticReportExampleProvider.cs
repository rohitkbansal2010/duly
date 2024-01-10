// <copyright file="DiagnosticReportExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Duly.Clinic.Api.ExampleProviders
{
    public class DiagnosticReportExampleProvider : IExamplesProvider<DiagnosticReport>
    {
        public DiagnosticReport GetExamples()
        {
            DateTime refTime = DateTime.UtcNow;

            return new DiagnosticReport
            {
                Id = "test",
                Name = "RAPID SARS-COV-2 BY PCR",
                EffectiveDate = refTime.AddDays(-1),
                Issued = refTime,
                Observations = new ObservationLabResult[]
                {
                    new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ComponentName = "Rapid SARS-CoV-2 by PCR",
                        Date = refTime,
                        Components = new ObservationLabResultComponent[]
                        {
                            new()
                            {
                                ValueText = "Not Detected",
                                Notes = new[]
                                {
                                    $"This test is intended for the qualitative detection of nucleic acid from the SARS-CoV-2 viral RNA from individuals who are suspected of COVID-19 infection by their healthcare provider.\r\n\r\nA \"Detected\" result is considered a positive test result for COVID-19. \r\nA \"Not Detected\" result for this test means that SARS-CoV-2 RNA was not present in the sample above the limit of detection of the assay.\r\n\r\nTest performed using the Abbott ID NOW COVID-19 assay performed on the ID NOW Instrument, Abbott Diagnostics Scarborough, Inc.; Scarborough, Maine 04074.\r\n\r\nThis test is being used under the Food and Drug Administration's Emergency Use Authorization.\r\n\r\nThe authorized Fact Sheet for Healthcare Providers for this assay is available upon request from the laboratory.\r\n\r\nEdward-Elmhurst Health Center - Downers Grove Laboratory\r\n2205 Butterfield Road, 60515, IL\r\nCLIA - 14D2183083"
                                },
                                ReferenceRange = new[]
                                {
                                    new ObservationLabResultReferenceRange
                                    {
                                        Text = "Not Detected"
                                    }
                                }
                            }
                        }
                    }
                },
                Performers = new[] { new PractitionerGeneralInfoExampleProvider().GetExamples() },
                Status = DiagnosticReportStatus.Final
            };
        }
    }
}