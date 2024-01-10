// <copyright file="ScheduledProvider.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;

namespace Duly.Clinic.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class ScheduledProviderExampleProvider : IExamplesProvider<ScheduledProvider>
    {
        public ScheduledProvider GetExamples()
        {
            return new()
            {
                DisplayName = "Mark P Nelson, MD",
                Identifiers = new[]
                {
                    "NPISER|1942277942",
                    "Dphone|1081",
                    "DATACHECK|1405",
                    "#61|12832552",
                    "#125|1405",
                    "DNPROVID|1405",
                    "#703|810457",
                    "#704|ZZNELSONMP",
                    "#163|810457",
                    "SPI|6179101491001",
                    "Internal|1405",
                    "External|1405"
                }
            };
        }
    }
}
