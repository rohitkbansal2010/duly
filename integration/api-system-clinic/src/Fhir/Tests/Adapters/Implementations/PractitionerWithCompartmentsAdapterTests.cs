// <copyright file="PractitionerWithCompartmentsAdapterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Fhir.Adapter.Adapters.Implementations;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Extensions;
using FluentAssertions;
using Hl7.Fhir.Rest;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;

namespace Fhir.Adapter.Tests.Adapters.Implementations
{
    [TestFixture]
    public class PractitionerWithCompartmentsAdapterTests
    {
        [Test]
        public async Task FindPractitionerWithRolesAsyncTest()
        {
            //Arrange
            var mockedClient = MockFindPractitionerRolesAsync(out var components);

            var searchCriteria = new SearchCriteria
            {
                SiteId = Guid.NewGuid().ToString()
            };

            var mockedBuilder = MockExtractPractitionerWithRolesAsync(
                components,
                out var practitionerWithRolesArray);

            IPractitionerWithCompartmentsAdapter adapter = new PractitionerWithCompartmentsAdapter(mockedClient.Object, mockedBuilder.Object);

            //Act
            var result = await adapter.FindPractitionersWithRolesAsync(searchCriteria);

            //Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);
            result.First().Should().Be(practitionerWithRolesArray[0]);
        }

        [Test]
        public async Task FindPractitionersByIdentifiersAsyncTest()
        {
            //Arrange
            var identifiers = new[] { "EXTERNAL|7650082", "EXTERNAL|7650074" };
            var mockedClient = MockFindPractitionerRolesAsync(out var components);

            var mockedBuilder = MockExtractPractitionerWithRolesAsync(
                components,
                out var practitionerWithRolesArray);

            IPractitionerWithCompartmentsAdapter adapter = new PractitionerWithCompartmentsAdapter(mockedClient.Object, mockedBuilder.Object);

            //Act
            var result = await adapter.FindPractitionersByIdentifiersAsync(identifiers);

            //Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);
            result.First().Should().Be(practitionerWithRolesArray[0]);
        }

        private static Mock<IPractitionerWithRolesBuilder> MockExtractPractitionerWithRolesAsync(
            R4.Bundle.EntryComponent[] components,
            out PractitionerWithRoles[] practitionerWithRolesArray)
        {
            practitionerWithRolesArray = new PractitionerWithRoles[]
            {
                new ()
                {
                    Resource = (R4.Practitioner)components[0].Resource
                }
            };

            var mockedBuilder = new Mock<IPractitionerWithRolesBuilder>();

            mockedBuilder
                .Setup(builder => builder.ExtractPractitionerWithRoles(components, true))
                .Returns(practitionerWithRolesArray);

            mockedBuilder
                .Setup(builder => builder.RetrievePractitionerWithRolesSafeAsync(components, false))
                .Returns(Task.FromResult(practitionerWithRolesArray));

            return mockedBuilder;
        }

        private static Mock<IFhirClientR4> MockFindPractitionerRolesAsync(out R4.Bundle.EntryComponent[] components)
        {
            components = new R4.Bundle.EntryComponent[]
            {
                new ()
                {
                    Resource = new R4.Practitioner()
                }
            };

            var mockedClient = new Mock<IFhirClientR4>();

            mockedClient
                .Setup(client => client.FindPractitionersAsync(It.IsAny<SearchParams>()))
                .Returns(Task.FromResult(components));

            mockedClient
                .Setup(client => client.FindPractitionerRolesAsync(It.IsAny<SearchParams>()))
                .Returns(Task.FromResult(components));

            return mockedClient;
        }
    }
}
