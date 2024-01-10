// <copyright file="Immunization.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Duly.Ngdp.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "ExampleProviders")]
    public class ImmunizationExamplesProvider : IExamplesProvider<IEnumerable<Immunization>>
    {
        public IEnumerable<Immunization> GetExamples()
        {
            yield return new Immunization
            {
                DueDate = DateTimeOffset.Now,
                VaccineName = "Influenza Vaccine",
                Status = DueStatus.Overdue,
                Patient = new Patient
                {
                    Id = "21355"
                }
            };
            yield return new Immunization
            {
                DueDate = DateTimeOffset.Now.AddDays(10),
                VaccineName = "Influenza Vaccine",
                Status = DueStatus.NotDue,
                Patient = new Patient
                {
                    Id = "21456"
                }
            };
        }
    }
}
