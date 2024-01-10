// <copyright file="ScheduledPatient.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;

namespace Duly.Clinic.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class ScheduledPatientExampleProvider : IExamplesProvider<ScheduledPatient>
    {
        public ScheduledPatient GetExamples()
        {
            return new()
            {
                Name = "Test1 Epam",
                Identifiers = new[]
                {
                    "EPI|3263815",
                    "EDW/LINDEN HOSP MRN|EH3014815",
                    "#84|3014815",
                    "#400|H550013815",
                    "#78|550013815",
                    "GEC CHART ID|GE11984815",
                    "BBEPI|100003815",
                    "Internal|7650074",
                    "External|7650074"
                }
            };
        }
    }
}
