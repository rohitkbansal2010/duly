// <copyright file="FhirDiagnosticReportRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;
using AutoMapper;
using Duly.Clinic.Api.Repositories.Implementations;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Common.Infrastructure.Exceptions;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Implementations
{
    extern alias stu3;

    [TestFixture]
    public class FhirDiagnosticReportRepositoryTests
    {
        private Mock<IMapper> _mapperMocked;
        private Mock<IDiagnosticReportWithCompartmentsAdapter> _adapterMocked;

        [SetUp]
        public void SetUp()
        {
            _adapterMocked = new Mock<IDiagnosticReportWithCompartmentsAdapter>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]
        public async Task FindDiagnosticReportsForPatientAsyncTest()
        {
            //Arrange
            const string patientId = "testId";
            var start = DateTimeOffset.Now;
            var end = DateTimeOffset.Now;

            var diagnosticReportWithCompartments = SetUpAdapter();
            var expectedCondition = SetUpMapper(diagnosticReportWithCompartments);

            IDiagnosticReportRepository repository = new FhirDiagnosticReportRepository(_adapterMocked.Object, _mapperMocked.Object);

            //Act
            var results = await repository.FindDiagnosticReportsForPatientAsync(patientId, start, end);

            //Assert
            _adapterMocked.Verify(
                x => x.FindDiagnosticReportsWithCompartmentsAsync(
                    It.Is<DiagnosticReportSearchCriteria>(
                        p => p.PatientId == patientId
                             && p.StartDateTime == start
                             && p.EndDateTime == end)),
                Times.Once());

            _mapperMocked.Verify(
                x => x.Map<IEnumerable<DiagnosticReport>>(
                    It.IsAny<IEnumerable<DiagnosticReportWithCompartments>>()),
                Times.Once());

            results.Should().NotBeNullOrEmpty();
            results.Should().HaveCount(expectedCondition.Length);
            results.First().Id.Should().Be(diagnosticReportWithCompartments.First().Resource.Id);
        }

        [Test]
        public async Task FindDiagnosticReportWithCompartmentsByIdAsyncTest()
        {
            //Arrange
            const string reportId = "testId";

            var fhirReport = new DiagnosticReportWithCompartments
            {
                Resource = new R4.DiagnosticReport
                {
                    Id = "Final1",
                    Status = R4.DiagnosticReport.DiagnosticReportStatus.Final
                }
            };
            _adapterMocked
                .Setup(adapter => adapter.FindDiagnosticReportWithCompartmentsByIdAsync(reportId))
                .Returns(Task.FromResult(fhirReport));

            var systemReport = new DiagnosticReport
            {
                Id = fhirReport.Resource.Id
            };
            _mapperMocked
                .Setup(mapper => mapper.Map<DiagnosticReport>(
                    It.IsAny<DiagnosticReportWithCompartments>()))
                .Returns(systemReport);

            IDiagnosticReportRepository repository = new FhirDiagnosticReportRepository(
                _adapterMocked.Object,
                _mapperMocked.Object);

            //Act
            var result = await repository.FindDiagnosticReportByIdAsync(reportId);

            //Assert
            _adapterMocked.Verify(
                x =>
                    x.FindDiagnosticReportWithCompartmentsByIdAsync(reportId),
                Times.Once());

            _mapperMocked.Verify(
                x => x.Map<DiagnosticReport>(
                    It.IsAny<DiagnosticReportWithCompartments>()),
                Times.Once());

            result.Should().NotBeNull();
        }

        [Test]
        public async Task FindDiagnosticReportByIdAsyncTest_EntityNotFoundException()
        {
            //Arrange
            const string reportId = "testId";
            IDiagnosticReportRepository repository = new FhirDiagnosticReportRepository(
                _adapterMocked.Object,
                _mapperMocked.Object);

            //Act
            Func<Task> action = async () => await repository.FindDiagnosticReportByIdAsync(reportId);

            //Assert
            var result = await action.Should().ThrowAsync<EntityNotFoundException>();
            result.Which.Message.Should().Be("DiagnosticReport with ID testId was not found.");
        }

        private IEnumerable<DiagnosticReportWithCompartments> SetUpAdapter()
        {
            IEnumerable<DiagnosticReportWithCompartments> results = new DiagnosticReportWithCompartments[]
            {
                new DiagnosticReportWithCompartments
                {
                    Resource = new R4.DiagnosticReport()
                    {
                        Id = "Entered in error",
                        Status = R4.DiagnosticReport.DiagnosticReportStatus.EnteredInError
                    }
                },
                new DiagnosticReportWithCompartments
                {
                    Resource = new R4.DiagnosticReport()
                    {
                        Id = "Final1",
                        Status = R4.DiagnosticReport.DiagnosticReportStatus.Final
                    }
                },
                new DiagnosticReportWithCompartments
                {
                    Resource = new R4.DiagnosticReport()
                    {
                        Id = "Final2",
                        Status = R4.DiagnosticReport.DiagnosticReportStatus.Final
                    }
                }
            };

            _adapterMocked
                .Setup(adapter => adapter.FindDiagnosticReportsWithCompartmentsAsync(It.IsAny<DiagnosticReportSearchCriteria>()))
                .Returns(Task.FromResult(results));

            return results;
        }

        private DiagnosticReport[] SetUpMapper(
            IEnumerable<DiagnosticReportWithCompartments> reportsWithCompartments)
        {
            var results = reportsWithCompartments
                .Select(item => new DiagnosticReport
                {
                    Id = item.Resource.Id
                })
                .ToArray();

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<DiagnosticReport>>(
                    It.IsAny<IEnumerable<DiagnosticReportWithCompartments>>()))
                .Returns(results);

            return results;
        }
    }
}
