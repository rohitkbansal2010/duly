// -----------------------------------------------------------------------
// <copyright file="AppointmentsControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Controllers;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
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
    public class AppointmentsControllerTests
    {
        private const string TestSiteId = "123";
        private const string TestAppointmentId = "appt_id";

        private readonly DateTimeOffset _testDateStart = DateTimeOffset.Now;
        private readonly DateTimeOffset _testDateEnd = DateTimeOffset.Now.AddDays(1);
        private AppointmentsController _controller;

        [SetUp]
        public void SetUp()
        {
            var loggerMock = new Mock<ILogger<AppointmentsController>>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            appointmentServiceMock.Setup(x => x.GetAppointmentsBySiteIdAndDateRangeAsync(TestSiteId, _testDateStart, _testDateEnd))
                .ReturnsAsync(new List<Appointment> { new () { Id = TestAppointmentId } });

            appointmentServiceMock.Setup(x => x.GetAppointmentsForSamePatientByAppointmentIdAsync(TestAppointmentId))
                .ReturnsAsync(PatientAppointments.BuildEmptyPatientAppointments);

            var iWebHostEnvironment = new Mock<IWebHostEnvironment>();
            _controller = new AppointmentsController(appointmentServiceMock.Object, loggerMock.Object, iWebHostEnvironment.Object);

            _controller.MockObjectValidator();
        }

        [Test]
        public void GetAppointmentsTest()
        {
            //Arrange
            ActionResult<IEnumerable<Appointment>> result = null;

            //Act
            Func<Task> act = async () => result = await _controller.GetAppointmentsForSiteByDateRange(TestSiteId, _testDateStart, _testDateEnd);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult.Value as IEnumerable<Appointment>;
            contentResult.Should().NotBeNullOrEmpty();
            contentResult.FirstOrDefault().Id.Should().Be(TestAppointmentId, "This is the one that is created in setup");
        }

        [Test]
        public void GetAppointmentsForSamePatientByAppointmentIdTest()
        {
            //Arrange
            ActionResult<PatientAppointments> result = null;

            //Act
            Func<Task> act = async () => result = await _controller.GetAppointmentsForSamePatientByAppointmentId(TestAppointmentId);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult.Value as PatientAppointments;
            contentResult.Should().NotBeNull();
        }
    }
}
