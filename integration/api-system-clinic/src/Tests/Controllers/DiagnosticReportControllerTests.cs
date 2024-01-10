// <copyright file="DiagnosticReportControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Controllers;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Common.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Tests.Controllers
{
    [TestFixture]
    public class DiagnosticReportControllerTests
    {
        private Mock<IDiagnosticReportRepository> _diagnosticReportRepositoryMocked;
        private Mock<ILogger<DiagnosticReportsController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _diagnosticReportRepositoryMocked = new Mock<IDiagnosticReportRepository>();
            _loggerMocked = new Mock<ILogger<DiagnosticReportsController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public async Task FindConditionsForPatientTest()
        {
            //Arrange
            const string patientId = "test";
            var start = DateTimeOffset.Now;
            var end = DateTimeOffset.Now;

            var diagnosticReports = ConfigureRepository(patientId, start, end);
            var controller = new DiagnosticReportsController( _diagnosticReportRepositoryMocked.Object, _loggerMocked.Object, _iWebHostEnvironment.Object);
            controller.MockObjectValidator();

            //Act
            var actionResult = await controller.FindDiagnosticReportsForPatient(patientId, start, end);

            //Assert
            _diagnosticReportRepositoryMocked.Verify(
                x => x.FindDiagnosticReportsForPatientAsync(
                    patientId,
                    start,
                    end),
                Times.Once());

            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<DiagnosticReport>)((OkObjectResult)actionResult.Result).Value;

            responseData.Should().NotBeNullOrEmpty();
            responseData.Should().HaveCount(diagnosticReports.Count());
            var firstItem = responseData.First();
            firstItem.Id.Should().Be(diagnosticReports.First().Id);
        }

        [Test]
        public async Task FindDiagnosticReportByIdTest()
        {
            //Arrange
            const string reportId = "test";

            var diagnosticReport = new DiagnosticReport
            {
                Id = Guid.NewGuid().ToString()
            };

            _diagnosticReportRepositoryMocked
                .Setup(repository => repository.FindDiagnosticReportByIdAsync(reportId))
                .Returns(Task.FromResult(diagnosticReport));

            var controller = new DiagnosticReportsController(_diagnosticReportRepositoryMocked.Object, _loggerMocked.Object, _iWebHostEnvironment.Object);
            controller.MockObjectValidator();

            //Act
            var actionResult = await controller.FindDiagnosticReportById(reportId);

            //Assert
            _diagnosticReportRepositoryMocked.Verify(
                x => x.FindDiagnosticReportByIdAsync(reportId),
                Times.Once());

            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (DiagnosticReport)((OkObjectResult)actionResult.Result).Value;

            responseData.Should().NotBeNull();
            responseData.Id.Should().Be(diagnosticReport.Id);
        }

        private IEnumerable<DiagnosticReport> ConfigureRepository(
            string patientId,
            DateTimeOffset start,
            DateTimeOffset end)
        {
            var diagnosticReports = new List<DiagnosticReport>
            {
                new DiagnosticReport
                {
                    Id = Guid.NewGuid().ToString()
                },
                new DiagnosticReport
                {
                    Id = Guid.NewGuid().ToString()
                },
                new DiagnosticReport
                {
                    Id = Guid.NewGuid().ToString()
                }
            };

            _diagnosticReportRepositoryMocked
                .Setup(repository => repository.FindDiagnosticReportsForPatientAsync(patientId, start, end))
                .Returns(Task.FromResult(diagnosticReports.AsEnumerable()));

            return diagnosticReports;
        }
    }
}