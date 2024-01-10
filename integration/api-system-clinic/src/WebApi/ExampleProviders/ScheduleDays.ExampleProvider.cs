// <copyright file="ScheduleDays.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Duly.Clinic.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class ScheduleDaysExampleProvider : IExamplesProvider<IEnumerable<ScheduleDay>>
    {
        public IEnumerable<ScheduleDay> GetExamples()
        {
            var slotsProvider = new SlotsExampleProvider();
            yield return new ScheduleDay
            {
                Date = new DateTimeOffset(2020, 1, 2, 0, 0, 0, default),
                Slots = slotsProvider.GetExamples().ToArray()
            };
        }
    }
}
