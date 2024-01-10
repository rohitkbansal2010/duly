// <copyright file="Slots.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Duly.Clinic.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class SlotsExampleProvider : IExamplesProvider<IEnumerable<Slot>>
    {
        public IEnumerable<Slot> GetExamples()
        {
            yield return new Slot
            {
                Time = TimeSpan.FromHours(5),
                ArrivalTime = TimeSpan.FromHours(6),
                DisplayTime = TimeSpan.FromHours(7),
            };
            yield return new Slot
            {
                Time = TimeSpan.FromHours(10),
                ArrivalTime = TimeSpan.FromHours(11),
                DisplayTime = TimeSpan.FromHours(12),
            };
        }
    }
}
