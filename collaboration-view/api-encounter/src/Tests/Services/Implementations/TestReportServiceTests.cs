// <copyright file="TestReportServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Implementations;
using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations
{
    [TestFixture]
    internal class TestReportServiceTests
    {
        private readonly DiagnosticReportStatus[] _reportStatusArray =
        {
            DiagnosticReportStatus.Registered,
            DiagnosticReportStatus.Partial,
            DiagnosticReportStatus.Amended,
            DiagnosticReportStatus.Appended,
            DiagnosticReportStatus.Corrected,
            DiagnosticReportStatus.Cancelled,
            DiagnosticReportStatus.Unknown,
            DiagnosticReportStatus.Preliminary,
            DiagnosticReportStatus.Final
        };

        private Mock<IDiagnosticReportRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IDiagnosticReportRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task GetHealthTestReportsByPatientIdAsyncTest()
        {
            //Arrange
            const int amount = 10;

            var patientId = Guid.NewGuid().ToString();
            var startDate = DateTime.Today.AddYears(-3).AsUtc();
            var endDate = DateTime.Today.AsUtc();
            var interval = new Interval(startDate, endDate);

            endDate = endDate.AddDays(1).AddMilliseconds(-1);
            var diagnosticReports = BuildDiagnosticReports(
                patientId,
                new(startDate, TimeSpan.Zero),
                new(endDate, TimeSpan.Zero));

            var expectedTestReports = ConfigureMapper(_reportStatusArray, diagnosticReports);

            var serviceMock = new TestReportService(
                _mapperMock.Object,
                _repositoryMock.Object);

            //Act
            var result = await serviceMock.GetTestReportsForPatientAsync(patientId, interval, amount);

            //Assert
            _repositoryMock.Verify(
                x => x.GetDiagnosticReportsForPatientAsync(patientId, startDate, endDate),
                Times.Once());

            _mapperMock.Verify(
                mapper => mapper.Map<IEnumerable<TestReport>>(
                    It.Is<IEnumerable<DiagnosticReport>>(
                        o => o.All(x => _reportStatusArray.Contains(x.Status)))),
                Times.Once());

            result.Should().NotBeNullOrEmpty();
            result.Should().AllBeOfType<TestReport>();
            result.Should().HaveCount(expectedTestReports.Count());
            result.First().Title.Should().Be(expectedTestReports.First().Title);
        }

        [TestCase(DiagnosticReportStatus.Amended)]
        [TestCase(DiagnosticReportStatus.Appended)]
        [TestCase(DiagnosticReportStatus.Cancelled)]
        [TestCase(DiagnosticReportStatus.Corrected)]
        [TestCase(DiagnosticReportStatus.Final)]
        [TestCase(DiagnosticReportStatus.Partial)]
        [TestCase(DiagnosticReportStatus.Preliminary)]
        [TestCase(DiagnosticReportStatus.Registered)]
        [TestCase(DiagnosticReportStatus.Unknown)]
        [TestCase(DiagnosticReportStatus.EnteredInError)]
        public async Task GetTestReportWithResultsByIdAsync_DiagnosticReportFound_Test(DiagnosticReportStatus diagnosticReportStatus)
        {
            //Arrange
            var reportId = Guid.NewGuid().ToString();
            var diagnosticReport = BuildDiagnosticReport(reportId, diagnosticReportStatus);
            var expectedTestReport = ConfigureMapper(_reportStatusArray, diagnosticReport);

            var serviceMock = new TestReportService(
                _mapperMock.Object,
                _repositoryMock.Object);

            //Act
            var result = await serviceMock.GetTestReportWithResultsByIdAsync(reportId);

            //Assert
            _repositoryMock.Verify(
                x => x.GetDiagnosticReportByIdAsync(reportId),
                Times.Once());

            if (_reportStatusArray.Contains(diagnosticReportStatus))
            {
                _mapperMock.Verify(
                    mapper => mapper.Map<TestReportWithResults>(
                        It.Is<DiagnosticReport>(
                            x => _reportStatusArray.Contains(x.Status))),
                    Times.Once());

                result.Should().NotBeNull();
                result.Title.Should().Be(expectedTestReport.Title);
            }
            else
            {
                _mapperMock.Verify(
                    mapper => mapper.Map<TestReportWithResults>(
                        It.IsAny<DiagnosticReport>()),
                    Times.Never());

                result.Should().BeNull();
            }
        }

        [Test]
        public async Task GetTestReportWithResultsByIdAsync_DiagnosticReportNotFound_Test()
        {
            //Arrange
            var reportId = Guid.NewGuid().ToString();
            var expectedTestReport = ConfigureMapper(_reportStatusArray, default(DiagnosticReport));

            var serviceMock = new TestReportService(
                _mapperMock.Object,
                _repositoryMock.Object);

            //Act
            var result = await serviceMock.GetTestReportWithResultsByIdAsync(reportId);

            //Assert
            _repositoryMock.Verify(
                x => x.GetDiagnosticReportByIdAsync(reportId),
                Times.Once());

            _mapperMock.Verify(
                mapper => mapper.Map<TestReportWithResults>(
                    It.IsAny<DiagnosticReport>()),
                Times.Never());

            result.Should().BeNull();
        }

        private IEnumerable<DiagnosticReport> BuildDiagnosticReports(
            string patientId,
            DateTimeOffset startDate,
            DateTimeOffset endDate)
        {
            var diagnosticReports = Enum.GetValues<DiagnosticReportStatus>()
                .Select(x => new DiagnosticReport
                {
                    Id = Guid.NewGuid().ToString(),
                    Status = x,
                    Name = $"Report with status: {x}",
                    EffectiveDate = endDate
                });

            _repositoryMock
                .Setup(repo => repo.GetDiagnosticReportsForPatientAsync(patientId, startDate, endDate))
                .Returns(Task.FromResult(diagnosticReports));

            return diagnosticReports;
        }

        private IEnumerable<TestReport> ConfigureMapper(
            DiagnosticReportStatus[] reportStatusArray,
            IEnumerable<DiagnosticReport> diagnosticReports)
        {
            var filteredDiagnosticReports = diagnosticReports
                .Where(x => reportStatusArray.Contains(x.Status));

            var testReports = filteredDiagnosticReports
                .Select(x => new TestReport
                {
                    Id = x.Id,
                    Title = x.Name,
                    Date = x.EffectiveDate.Value
                });

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<TestReport>>(It.IsAny<IEnumerable<DiagnosticReport>>()))
                .Returns(testReports);

            return testReports
                .OrderByDescending(x => x.Date)
                .ThenBy(x => x.Title);
        }

        private DiagnosticReport BuildDiagnosticReport(string reportId, DiagnosticReportStatus diagnosticReportStatus)
        {
            var diagnosticReport = new DiagnosticReport
                {
                    Id = reportId,
                    Status = diagnosticReportStatus,
                    Name = $"Report with status: {DiagnosticReportStatus.Final}",
                    EffectiveDate = DateTimeOffset.UtcNow.AddHours(-1),
                    Issued = DateTimeOffset.UtcNow
                };

            _repositoryMock
                .Setup(repo => repo.GetDiagnosticReportByIdAsync(reportId))
                .Returns(Task.FromResult(diagnosticReport));

            return diagnosticReport;
        }

        private TestReportWithResults ConfigureMapper(
            DiagnosticReportStatus[] reportStatusArray,
            DiagnosticReport diagnosticReport)
        {
            TestReportWithResults testReportWithResults = null;

            if (diagnosticReport != null)
            {
                testReportWithResults = new TestReportWithResults
                {
                    Id = diagnosticReport.Id,
                    Title = diagnosticReport.Name,
                    EffectiveDate = diagnosticReport.EffectiveDate.Value,
                    Issued = diagnosticReport.Issued
                };
            }

            _mapperMock
                .Setup(mapper => mapper.Map<TestReportWithResults>(It.IsAny<DiagnosticReport>()))
                .Returns(testReportWithResults);

            return testReportWithResults;
        }
    }
}
