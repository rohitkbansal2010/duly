// <copyright file="DiagnosticReportWithCompartmentsBuilderTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Fhir.Adapter.Builders.Implementations;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;

namespace Fhir.Adapter.Tests.Builders.Implementations
{
    [TestFixture]
    public class DiagnosticReportWithCompartmentsBuilderTests
    {
        [Test]
        public async Task ExtractDiagnosticReportWithCompartmentsAsyncTest()
        {
            //Arrange
            var practitionerId = Guid.NewGuid().ToString();
            var diagnosticId = Guid.NewGuid().ToString();
            var observationId = Guid.NewGuid().ToString();
            var components = SetComponents(diagnosticId, practitionerId, observationId);

            var mockedPractitionerWithRolesBuilder = MockRetrievePractitionerWithRolesAsync(components, practitionerId);

            IDiagnosticReportWithCompartmentsBuilder builder = new DiagnosticReportWithCompartmentsBuilder(
                mockedPractitionerWithRolesBuilder.Object);

            //Act
            var result = await builder.ExtractDiagnosticReportWithCompartmentsAsync(components);

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.First().Resource.Should().NotBeNull();
            result.First().Resource.Id.Should().Be(diagnosticId);
            result.First().Performers.Should().NotBeNullOrEmpty();
            result.First().Performers.Should().HaveCount(1);
            result.First().Performers.First().Resource.Should().NotBeNull();
            result.First().Performers.First().Resource.Id.Should().Be(practitionerId);
            result.First().Observations.Should().NotBeNullOrEmpty();
            result.First().Observations.Should().HaveCount(1);
            result.First().Observations.First().Should().NotBeNull();
            result.First().Observations.First().Id.Should().Be(observationId);
        }

        [Test]
        public async Task ExtractDiagnosticReportWithCompartmentsAsyncTest_EmptyComponents()
        {
            //Arrange
            var components = Array.Empty<R4.Bundle.EntryComponent>();

            var mockedPractitionerWithRolesBuilder = new Mock<IPractitionerWithRolesBuilder>();

            IDiagnosticReportWithCompartmentsBuilder builder = new DiagnosticReportWithCompartmentsBuilder(
                mockedPractitionerWithRolesBuilder.Object);

            //Act
            var result = await builder.ExtractDiagnosticReportWithCompartmentsAsync(components);

            //Assert
            result.Should().BeEmpty();
        }

        [Test]
        public async Task ExtractDiagnosticReportWithCompartmentsByIdAsyncTest()
        {
            //Arrange
            var practitionerId = Guid.NewGuid().ToString();
            var diagnosticId = Guid.NewGuid().ToString();
            var observationId = Guid.NewGuid().ToString();
            var components = SetComponents(diagnosticId, practitionerId, observationId);

            var mockedPractitionerWithRolesBuilder = MockRetrievePractitionerWithRolesAsync(components, practitionerId);

            IDiagnosticReportWithCompartmentsBuilder builder = new DiagnosticReportWithCompartmentsBuilder(
                mockedPractitionerWithRolesBuilder.Object);

            //Act
            var result = await builder.ExtractDiagnosticReportWithCompartmentsByIdAsync(components);

            //Assert
            result.Should().NotBeNull();
            result.Resource.Should().NotBeNull();
            result.Resource.Id.Should().Be(diagnosticId);
            result.Performers.Should().NotBeNullOrEmpty();
            result.Performers.Should().HaveCount(1);
            result.Performers.First().Resource.Should().NotBeNull();
            result.Performers.First().Resource.Id.Should().Be(practitionerId);
            result.Observations.Should().NotBeNullOrEmpty();
            result.Observations.Should().HaveCount(1);
            result.Observations.First().Should().NotBeNull();
            result.Observations.First().Id.Should().Be(observationId);
        }

        [Test]
        public async Task ExtractDiagnosticReportWithCompartmentsByIdAsyncTest_ReturnNull()
        {
            //Arrange
            var components = Array.Empty<R4.Bundle.EntryComponent>();

            var mockedPractitionerWithRolesBuilder = new Mock<IPractitionerWithRolesBuilder>();

            IDiagnosticReportWithCompartmentsBuilder builder = new DiagnosticReportWithCompartmentsBuilder(
                mockedPractitionerWithRolesBuilder.Object);

            //Act
            var result = await builder.ExtractDiagnosticReportWithCompartmentsByIdAsync(components);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task ExtractDiagnosticReportWithCompartmentsByIdAsyncTest_InvalidOperationException()
        {
            //Arrange
            var practitionerId = Guid.NewGuid().ToString();

            var components = new R4.Bundle.EntryComponent[]
            {
                new ()
                {
                    Resource = new R4.DiagnosticReport { Id = Guid.NewGuid().ToString() }
                },
                new ()
                {
                    Resource = new R4.DiagnosticReport { Id = Guid.NewGuid().ToString() }
                },
            };

            var mockedPractitionerWithRolesBuilder = MockRetrievePractitionerWithRolesAsync(components, practitionerId);

            IDiagnosticReportWithCompartmentsBuilder builder = new DiagnosticReportWithCompartmentsBuilder(
                mockedPractitionerWithRolesBuilder.Object);

            //Act
            Func<Task> action = async () => await builder.ExtractDiagnosticReportWithCompartmentsByIdAsync(components);

            //Assert
            var result = await action.Should().ThrowAsync<InvalidOperationException>();
            result.Which.Message.Should().Be("Sequence contains more than one element");
        }

        private static R4.Bundle.EntryComponent[] SetComponents(string diagnosticId, string practitionerId, string observationId)
        {
            return new R4.Bundle.EntryComponent[]
            {
                new()
                {
                    Resource = new R4.DiagnosticReport
                    {
                        Id = diagnosticId,
                        Performer = new()
                        {
                            new (nameof(R4.Practitioner) + "/" + practitionerId)
                        },
                        Result = new()
                        {
                            new(nameof(R4.Observation) + "/" + observationId)
                        }
                    }
                },
                new()
                {
                    Resource = new R4.Observation { Id = observationId }
                }
            };
        }

        private static Mock<IPractitionerWithRolesBuilder> MockRetrievePractitionerWithRolesAsync(R4.Bundle.EntryComponent[] components, string practitionerId)
        {
            var mockedPractitionerWithRolesBuilder = new Mock<IPractitionerWithRolesBuilder>();

            var practitionersWithRoles = new PractitionerWithRoles[]
            {
                new()
                {
                    Resource = new() { Id = practitionerId },
                    Roles = new[]
                    {
                        new R4.PractitionerRole { Id = Guid.NewGuid().ToString() },
                    }
                }
            };
            var searchResults = components.ToArray();
            mockedPractitionerWithRolesBuilder
                .Setup(builder => builder.RetrievePractitionerWithRolesAsync(searchResults, false))
                .Returns(Task.FromResult(practitionersWithRoles));

            return mockedPractitionerWithRolesBuilder;
        }
    }
}
