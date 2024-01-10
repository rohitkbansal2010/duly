// <copyright file="OrganizationWithCompartmentsBuilderTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Fhir.Adapter.Builders.Implementations;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

using R4 = r4::Hl7.Fhir.Model;

namespace Fhir.Adapter.Tests.Builders.Implementations
{
    [TestFixture]
    public class OrganizationWithCompartmentsBuilderTests
    {
        [Test]
        public void ExtractOrganizationsTest()
        {
            //Arrange
            var organizationId = Guid.NewGuid().ToString();
            var components = new R4.Bundle.EntryComponent[]
            {
                new()
                {
                    Resource = new R4.Organization { Id = organizationId }
                }
            };

            IOrganizationWithCompartmentsBuilder builder = new OrganizationWithCompartmentsBuilder();

            //Act
            var result = builder.ExtractOrganizations(components);

            //Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);
            result.First().Resource.Id.Should().Be(organizationId);
        }
    }
}
