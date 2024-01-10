// <copyright file="ScheduledVisitType.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Duly.Clinic.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class ScheduledVisitTypeExampleProvider : IExamplesProvider<ScheduledVisitType>
    {
        public ScheduledVisitType GetExamples()
        {
            return new()
            {
                Identifiers = new[]
                {
                    "Internal|2147",
                    "External|2147"
                },
                Name = "MEDICARE AHA PREVENTIVE VISIT",
                DisplayName = "Medicare Annual Health Assessment Visit",
                PatientInstructions = Array.Empty<string>()
            };
        }
    }
}
