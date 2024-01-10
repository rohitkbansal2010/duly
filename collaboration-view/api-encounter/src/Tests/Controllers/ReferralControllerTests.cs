// <copyright file="ReferralControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

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
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Controllers
{
    [TestFixture]
    [SetCulture("en-us")]
    public class ReferralControllerTests
    {
        private Mock<ILogger<ReferralController>> _loggerMock;
        private Mock<IReferralService> _referralServiceMock;
        private Mock<IScheduleSlotsservice> _scheduleSlotsserviceMock;
        private Mock<IGetSlotsservice> _getSlotsserviceMock;
        private ReferralController _controller;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<ReferralController>>();
            _referralServiceMock = new Mock<IReferralService>();
            _scheduleSlotsserviceMock = new Mock<IScheduleSlotsservice>();
            _getSlotsserviceMock = new Mock<IGetSlotsservice>();

            var iWebHostEnvironment = new Mock<IWebHostEnvironment>();

            _controller = new ReferralController(
                _referralServiceMock.Object,
                _scheduleSlotsserviceMock.Object,
                _getSlotsserviceMock.Object,
                _loggerMock.Object,
                iWebHostEnvironment.Object);
        }

        /// <summary>
        /// 1.  should call "PostReferral" and return string result data<see cref="PostReferralSkippedSuccessTest"/>.
        /// </summary>
        [Test]
        public void PostReferralSuccessTest()
        {
            //Arrange
            _ = PostReferralSkippedSuccessSetup();

            ActionResult<ScheduleAppointmentResult> result = null;
            _controller.MockObjectValidator();

            //Act
            ScheduleReferral scheduleReferral = new ScheduleReferral()
            {
                Appointment_Id = "7000011",
                AptFormat = "7000012",
                AptScheduleDate = DateTime.Now,
                Patient_ID = "1",
                BookingSlot = "10:00",
                Skipped = true
            };

            Func<Task> act = async () => result = await _controller.PostReferral(scheduleReferral);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value;
            contentResult.Should().NotBeNull();
            contentResult.Should().BeOfType<string>();
        }

        /// <summary>
        /// 2.  should call "PostReferral" and return string result data<see cref="PostReferralWithoutSkippedSuccessTest"/>.
        /// </summary>
        [Test]
        public void PostReferralWithoutSkippedSuccessTest()
        {
            ActionResult<ScheduleAppointmentResult> result = null;
            _controller.MockObjectValidator();

            ScheduleReferral scheduleReferral = new ScheduleReferral()
            {
                Appointment_Id = "7000011",
                AptFormat = "7000012",
                AptScheduleDate = DateTime.Now,
                Patient_ID = "1",
                BookingSlot = "10:00:00",
                Provider_ID = "TestProviderId",
                Location_ID = "TestLocationId",
                VisitTypeId = "TestVisitTypeId",
                Skipped = false
            };

            var record_ID = PostReferralWithoutSkippedSuccessSetup(scheduleReferral);
            _ = ScheduleAppointmentForPatientSetup();
            _ = DataPostedToEpicAsyncSetup(record_ID);

            //Act
            Func<Task> act = async () => result = await _controller.PostReferral(scheduleReferral);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value as ActionResult<ScheduleAppointmentResult>;
            contentResult.Value.Should().NotBeNull();
            contentResult.Value.Should().BeOfType<ScheduleAppointmentResult>();
        }

        private int PostReferralSkippedSuccessSetup()
        {
            ScheduleReferral scheduleReferral = new ScheduleReferral()
            {
                Appointment_Id = "7000011",
                AptFormat = "7000012",
                AptScheduleDate = DateTime.Now,
                Patient_ID = "1",
                BookingSlot = "BookingSlots",
                Skipped = true
            };

            _referralServiceMock
                .Setup(x => x.PostReferral(scheduleReferral))
                .Returns(Task.FromResult(1));

            return 1;
        }

        private int PostReferralWithoutSkippedSuccessSetup(ScheduleReferral scheduleReferral)
        {
            _referralServiceMock
                .Setup(x => x.PostReferral(scheduleReferral))
                .Returns(Task.FromResult(1));

            return 1;
        }

        private ScheduleAppointmentResult ScheduleAppointmentForPatientSetup()
        {
            ScheduleAppointmentResult response = new ScheduleAppointmentResult()
            {
                Id = "TestId",
                DateTime = DateTime.Now,
                DurationInMinutes = 10,
                ScheduledTime = DateTime.Now,
                TimeZone = "TimeZone"
            };

            _scheduleSlotsserviceMock
                .Setup(x => x.ScheduleReferralAppointmentForPatientAsync(It.IsAny<AppointmentSchedulingModel>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(response));

            return response;
        }

        private int DataPostedToEpicAsyncSetup(int recordId)
        {
            _referralServiceMock
                .Setup(x => x.DataPostedToEpicAsync(recordId))
                .Returns(Task.FromResult(1));

            return 1;
        }
    }
}