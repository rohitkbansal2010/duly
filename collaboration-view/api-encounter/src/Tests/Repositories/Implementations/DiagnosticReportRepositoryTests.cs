// <copyright file="DiagnosticReportRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Implementations;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class DiagnosticReportRepositoryTests
    {
        private Mock<IEncounterContext> _encounterContextMock;
        private Mock<IPatientsClient> _patientsClientMock;
        private Mock<IClient> _commonClientMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            ConfigureEncounterContext();
            _patientsClientMock = new Mock<IPatientsClient>();
            _commonClientMock = new Mock<IClient>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task GetDiagnosticReportsForPatientAsync_Test()
        {
            //Arrange
            var patientId = Guid.NewGuid().ToString();
            DateTimeOffset startDate = DateTime.Today.AddYears(-3);
            DateTimeOffset endDate = DateTime.Today;

            var systemDiagnosticReports = BuildDiagnosticReports(patientId, startDate, endDate);

            ConfigureMapper(systemDiagnosticReports);

            var repository = new DiagnosticReportRepository(
                _encounterContextMock.Object,
                _mapperMock.Object,
                _patientsClientMock.Object,
                _commonClientMock.Object);

            //Act
            var result = await repository.GetDiagnosticReportsForPatientAsync(patientId, startDate, endDate);

            //Assert
            _patientsClientMock.Verify(
                x => x.DiagnosticReportsAsync(
                    patientId,
                    startDate,
                    endDate,
                    It.IsAny<Guid>(),
                    default),
                Times.Once());

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(systemDiagnosticReports.Count);
            result.Last().Id.Should().Be(systemDiagnosticReports.Last().Id);
        }

        [Test]
        public async Task GetDiagnosticReportByIdAsync_Test()
        {
            //Arrange
            var reportId = Guid.NewGuid().ToString();

            var systemDiagnosticReport = BuildDiagnosticReport(reportId);

            ConfigureMapper(systemDiagnosticReport);

            var repository = new DiagnosticReportRepository(
                _encounterContextMock.Object,
                _mapperMock.Object,
                _patientsClientMock.Object,
                _commonClientMock.Object);

            //Act
            var result = await repository.GetDiagnosticReportByIdAsync(reportId);

            //Assert
            _commonClientMock.Verify(
                x => x.DiagnosticReportsAsync(
                    reportId,
                    It.IsAny<Guid>(),
                    default),
                Times.Once());

            result.Should().NotBeNull();
            result.Id.Should().Be(systemDiagnosticReport.Id);
        }

        private void ConfigureEncounterContext()
        {
            _encounterContextMock = new Mock<IEncounterContext>();

            _encounterContextMock
                .Setup(ctx => ctx.GetXCorrelationId())
                .Returns(Guid.NewGuid());
        }

        private ICollection<DiagnosticReport> BuildDiagnosticReports(
            string patientId,
            DateTimeOffset startDate,
            DateTimeOffset endDate)
        {
            ICollection<DiagnosticReport> diagnosticReports = new DiagnosticReport[]
            {
                new ()
                {
                    Id = Guid.NewGuid().ToString()
                },
                new ()
                {
                    Id = Guid.NewGuid().ToString()
                },
                new ()
                {
                    Id = Guid.NewGuid().ToString()
                }
            };

            _patientsClientMock
                .Setup(client => client.DiagnosticReportsAsync(patientId, startDate, endDate, It.IsAny<Guid>(), default))
                .Returns(Task.FromResult(diagnosticReports));

            return diagnosticReports;
        }

        private void ConfigureMapper(ICollection<DiagnosticReport> systemDiagnosticReports)
        {
            IEnumerable<Models.DiagnosticReport> diagnosticReports = systemDiagnosticReports
                .Select(x => new Models.DiagnosticReport
                {
                    Id = x.Id
                });

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<Models.DiagnosticReport>>(systemDiagnosticReports))
                .Returns(diagnosticReports);
        }

        private DiagnosticReport BuildDiagnosticReport(string reportId)
        {
            var diagnosticReport = new DiagnosticReport
            {
                Id = reportId
            };

            _commonClientMock
                .Setup(client => client.DiagnosticReportsAsync(reportId, It.IsAny<Guid>(), default))
                .Returns(Task.FromResult(diagnosticReport));

            return diagnosticReport;
        }

        private void ConfigureMapper(DiagnosticReport systemDiagnosticReport)
        {
            var diagnosticReport = new Models.DiagnosticReport
            {
                Id = systemDiagnosticReport.Id
            };

            _mapperMock
                .Setup(mapper => mapper.Map<Models.DiagnosticReport>(systemDiagnosticReport))
                .Returns(diagnosticReport);
        }
    }
}
