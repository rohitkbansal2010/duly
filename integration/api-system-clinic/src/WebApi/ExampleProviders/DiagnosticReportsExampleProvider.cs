// <copyright file="DiagnosticReportsExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace Duly.Clinic.Api.ExampleProviders
{
    public class DiagnosticReportsExampleProvider : IExamplesProvider<IEnumerable<DiagnosticReport>>
    {
        public IEnumerable<DiagnosticReport> GetExamples()
        {
            yield return new DiagnosticReportExampleProvider().GetExamples();
        }
    }
}