// <copyright file="ReferralPatient.ExamplesProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Duly.Ngdp.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "ExampleProviders")]
    public class ReferralPatientExamplesProvider : IExamplesProvider<ReferralPatient>
    {
        public ReferralPatient GetExamples()
        {
            return new ReferralPatient
            {
                Patient = new Patient { Id = "7650077" },
                Name = new HumanName { FamilyName = "Epam", GivenNames = new[] { "Test2" } },
                DateOfBirth = new DateTime(1987, 12, 20),
                Address = new Address { City = "Chicago", State = "IL", PostalCode = "60605", Lines = new[] { "123" } },
                ContactPoints = new[] { new ContactPoint { Type = ContactPointType.Phone, Value = "098-234-2342" } }
            };
        }
    }
}