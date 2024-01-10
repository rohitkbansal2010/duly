// <copyright file="DiagnosticReportWithCompartmentsAdapterTests.cs" company="Duly Health and Care">
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
    public class DiagnosticReportWithCompartmentsAdapterTests
    {
        private readonly Mock<IFhirClientR4> _mockedClient = new();
        private readonly Mock<IDiagnosticReportWithCompartmentsBuilder> _mockedBuilder = new();

        [Test]
        public async Task FindDiagnosticReportsWithCompartmentsAsyncTest()
        {
            var components = SetUpClient();

            var searchCriteria = new DiagnosticReportSearchCriteria
            {
                PatientId = Guid.NewGuid().ToString(),
            };

            var diagnosticReportWithCompartments = new DiagnosticReportWithCompartments[]
            {
                new ()
                {
                    Resource = (R4.DiagnosticReport)components[0].Resource
                }
            };

            _mockedBuilder
                .Setup(builder => builder.ExtractDiagnosticReportWithCompartmentsAsync(components))
                .ReturnsAsync(diagnosticReportWithCompartments);

            IDiagnosticReportWithCompartmentsAdapter adapter = new DiagnosticReportWithCompartmentsAdapter(
                _mockedClient.Object,
                _mockedBuilder.Object);

            //Act
            var result = await adapter.FindDiagnosticReportsWithCompartmentsAsync(searchCriteria);

            //Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);
            result.First().Should().Be(diagnosticReportWithCompartments[0]);
        }

        [Test]
        public async Task FindDiagnosticReportWithCompartmentsByIdAsyncTest()
        {
            var components = SetUpClient();

            var reportId = Guid.NewGuid().ToString();

            var diagnosticReportWithCompartments = new DiagnosticReportWithCompartments
            {
                Resource = (R4.DiagnosticReport)components[0].Resource
            };

            _mockedBuilder
                .Setup(builder => builder.ExtractDiagnosticReportWithCompartmentsByIdAsync(components))
                .ReturnsAsync(diagnosticReportWithCompartments);

            IDiagnosticReportWithCompartmentsAdapter adapter = new DiagnosticReportWithCompartmentsAdapter(
                _mockedClient.Object,
                _mockedBuilder.Object);

            //Act
            var result = await adapter.FindDiagnosticReportWithCompartmentsByIdAsync(reportId);

            //Assert
            result.Should().NotBeNull();
            result.Should().Be(diagnosticReportWithCompartments);
        }

        [Test]
        public async Task FindDiagnosticReportWithCompartmentsByIdAsyncTest_ReturnNull()
        {
            //Arrange
            var reportId = Guid.NewGuid().ToString();

            _mockedClient
                .Setup(client => client.FindDiagnosticReportsAsync(It.IsAny<SearchParams>()))
                .Returns(Task.FromResult(Array.Empty<R4.Bundle.EntryComponent>()));

            IDiagnosticReportWithCompartmentsAdapter adapter = new DiagnosticReportWithCompartmentsAdapter(
                _mockedClient.Object,
                _mockedBuilder.Object);

            //Act
            var result = await adapter.FindDiagnosticReportWithCompartmentsByIdAsync(reportId);

            //Assert
            result.Should().BeNull();
        }

        private R4.Bundle.EntryComponent[] SetUpClient()
        {
            var components = new R4.Bundle.EntryComponent[]
            {
                new ()
                {
                    Resource = new R4.DiagnosticReport
                    {
                        Id = Guid.NewGuid().ToString()
                    }
                }
            };

            _mockedClient
                .Setup(client => client.FindDiagnosticReportsAsync(It.IsAny<SearchParams>()))
                .Returns(Task.FromResult(components));

            return components;
        }
    }
}
