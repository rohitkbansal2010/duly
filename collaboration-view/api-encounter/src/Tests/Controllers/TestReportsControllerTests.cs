// <copyright file="TestReportsControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Controllers;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Infrastructure.Exceptions;
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

namespace Duly.CollaborationView.Encounter.Api.Tests.Controllers
{
    [TestFixture]
    [SetCulture("en-us")]
    public class TestReportsControllerTests
    {
        private Mock<ILogger<TestReportsController>> _loggerMock;
        private Mock<ITestReportService> _serviceMock;

        private TestReportsController _controller;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<TestReportsController>>();
            _serviceMock = new Mock<ITestReportService>();
            var iWebHostEnvironment = new Mock<IWebHostEnvironment>();

            _controller = new TestReportsController(_serviceMock.Object, _loggerMock.Object, iWebHostEnvironment.Object);
        }

        [Test]
        public async Task GetTestReportsTest()
        {
            //Arrange
            const int amount = 10;
            const string patientId = "test-patient-id";

            var startDate = DateTime.Today.AddYears(-3);
            var endDate = DateTime.Today;
            var interval = new Interval(startDate, endDate);

            var testReports = SetupService(patientId, interval, amount);

            _controller.MockObjectValidator();

            //Act
            var result = await _controller.GetTestReportsForPatient(patientId, startDate, endDate, amount);

            //Assert
            _serviceMock.Verify(
                x => x.GetTestReportsForPatientAsync(
                    patientId,
                    It.Is<Interval>(inter => inter.StartDate == interval.StartDate && inter.EndDate == interval.EndDate),
                    amount),
                Times.Once());

            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult.Value as IEnumerable<TestReport>;
            contentResult = contentResult.ToArray();
            contentResult.Should().NotBeNullOrEmpty();
            contentResult.Should().HaveCount(testReports.Count());
            contentResult.Last().Id.Should().Be(testReports.Last().Id);
        }

        [Test]
        public async Task GetTestReports_Throw_BadDataException_Test()
        {
            //Arrange
            const int amount = 10;
            const string patientId = "test-patient-id";

            var startDate = DateTime.Today.AddYears(-3);
            var endDate = DateTime.Today;

            MockIObjectModelValidator();

            //Act
            Func<Task> action = async () => await _controller.GetTestReportsForPatient(patientId, startDate, endDate, amount);

            //Assert\
            var result = await action.Should().ThrowAsync<BadDataException>();
            result.Which.Message.Should().Be($"In Test: Error message: Test error message, Exception: {Environment.NewLine}");
        }

        [Test]
        public async Task GetTestReportResultsByReportIdTest()
        {
            //Arrange
            const string reportId = "test-report-id";
            List<string> reportList = new List<string>();
            reportList.Add(reportId);

            var testReportWithResults = SetupService(reportId);

            _controller.MockObjectValidator();

            //Act
            var result = await _controller.GetTestReportResultsByReportId(reportId);

            //Assert
            _serviceMock.Verify(
                x => x.GetTestReportWithResultsByIdAsync(reportId),
                Times.Once());

            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult.Value;
            contentResult.Should().NotBeNull();
        }

        private IEnumerable<TestReport> SetupService(
            string patientId,
            Interval interval,
            int amount)
        {
            IEnumerable<TestReport> testReports = new TestReport[]
            {
                new()
                {
                    Id = Guid.NewGuid().ToString()
                },
                new()
                {
                    Id = Guid.NewGuid().ToString()
                },
                new()
                {
                    Id = Guid.NewGuid().ToString()
                }
            };

            _serviceMock
                .Setup(x => x.GetTestReportsForPatientAsync(
                    patientId,
                    It.Is<Interval>(inter => inter.StartDate == interval.StartDate && inter.EndDate == interval.EndDate),
                    amount))
                .Returns(Task.FromResult(testReports));

            return testReports;
        }

        private void MockIObjectModelValidator()
        {
            _controller.MockObjectValidator();
            _controller.ModelState.AddModelError("Test", "Test error message");
        }

        private TestReportWithResults SetupService(string reportId)
        {
            var reportWithResults = new TestReportWithResults
            {
                Id = reportId
            };

            _serviceMock
                .Setup(x => x.GetTestReportWithResultsByIdAsync(reportId))
                .Returns(Task.FromResult(reportWithResults));

            return reportWithResults;
        }
    }
}