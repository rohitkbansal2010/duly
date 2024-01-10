// <copyright file="Immunizations.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Duly.Clinic.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class ImmunizationsExampleProvider : IExamplesProvider<IEnumerable<Immunization>>
    {
        public IEnumerable<Immunization> GetExamples()
        {
            yield return new Immunization
            {
                Id = Guid.NewGuid().ToString(),
                Dose = new Quantity
                {
                    Value = 0.5m,
                    Unit = "mL"
                },
                OccurrenceDateTime = DateTimeOffset.Parse("2021-07-19"),
                Status = ImmunizationStatus.Completed,
                Vaccine = new Vaccine
                {
                    CvxCodes = new[] { "8987" },
                    Text = "Measles"
                }
            };

            yield return new Immunization
            {
                Id = Guid.NewGuid().ToString(),
                Dose = new Quantity
                {
                    Value = 1,
                    Unit = "puff"
                },
                OccurrenceDateTime = DateTimeOffset.Parse("2020-05-10"),
                Status = ImmunizationStatus.NotDone,
                Vaccine = new Vaccine
                {
                    CvxCodes = new[] { "8987" },
                    Text = "Measles"
                }
            };
        }
    }
}