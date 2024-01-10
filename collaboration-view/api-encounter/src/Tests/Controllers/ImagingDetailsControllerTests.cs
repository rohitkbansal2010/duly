// <copyright file="ImagingDetailsControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Controllers;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
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
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Controllers
{
    [TestFixture]
    [SetCulture("en-us")]
    public class ImagingDetailsControllerTests
    {
        //private const string PatientId = "test-patient-id";
        private const int CvcheckOutId = 2;
        private static ImagingDetail imagingDetail = new ImagingDetail
        {
            ImagingType = "I",
            Appointment_ID = "test-appointment-id",
            Patient_ID = "test-patient-id",
            Provider_ID = "test-provider-id",
            Location_ID = "test-location-id",
            BookingSlot = "test-booking-slot",
            AptScheduleDate = new System.DateTime(2008, 5, 1, 8, 6, 32),
            ImagingLocation = "test-imaging-location",
            Skipped = false
        };

        private Mock<ILogger<ImagingDetailController>> _loggerMock;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;
        private Mock<IScheduleSlotsservice> _scheduleservice;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<ImagingDetailController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
            _scheduleservice = new Mock<IScheduleSlotsservice>();
        }

        [Test]
        public void GetPatientByIdTest()
        {
            //Arrange
            ActionResult<CreationResultResponse> result = null;
            var controller = new ImagingDetailController(
                SetupPostLabDetailAsync(),
                _loggerMock.Object,
                _scheduleservice.Object,
                _iWebHostEnvironment.Object);

            controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await controller.PostImagingDetail(imagingDetail);
            act.Invoke();

            //Assert
            act.Should()
                .NotThrowAsync();

            var okResult = (ObjectResult)result.Result;
            var contentResult = okResult.Value;

            contentResult.Should().NotBeNull();
        }

        private static IImagingDetailService SetupPostLabDetailAsync()
        {
            var serviceMock = new Mock<IImagingDetailService>();

            serviceMock
                .Setup(x => x.PostImagingDetailAsync(imagingDetail))
                .Returns(Task.FromResult(CvcheckOutId));

            return serviceMock.Object;
        }

        [Test]
        public void ScheduleImagingAppointment()
        {
            //Arrange
            ActionResult<ScheduleAppointmentResult> result = null;
            var controller = new ImagingDetailController(
                SetupPostLabDetailAsync(),
                _loggerMock.Object,
                SetupScheduleImagingAsync(),
                _iWebHostEnvironment.Object);

            controller.MockObjectValidator();

            List<string> providerId = new();
            providerId.Add("test-provider-id");

            ImagingSchedulingModel imagingSchedulingRequest = new()
            {
                ProviderId = providerId,
                SelectedProviderId = "selected-test-provider-id",
                Date = new System.DateTime(2008, 5, 1, 8, 6, 32),
                DepartmentId = "External|test-deptId",
                Time = new System.TimeSpan(09, 00, 00),
                PatientId = "External|test-patient-Id"
            };

            //Act
            Func<Task> act = async () => result = await controller.ScheduleImagingAppointment(imagingSchedulingRequest);
            act.Invoke();

            //Assert
            act.Should()
                .NotThrowAsync();

            var okResult = (ObjectResult)result.Result;
            var contentResult = okResult.Value;

            contentResult.Should().NotBeNull();
        }

        private static IScheduleSlotsservice SetupScheduleImagingAsync()
        {
            var serviceMock = new Mock<IScheduleSlotsservice>();

            ScheduleAppointmentModel appointmentSchedulingRequest = new()
            {
                ProviderId = "External|test-providerId",
                Date = new System.DateTime(2008, 5, 1, 8, 6, 32),
                DepartmentId = "External|test-deptId",
                Time = new System.TimeSpan(09, 00, 00),
                VisitTypeId = "External|4576",
                PatientId = "External|test-patient-Id"
            };

            ScheduleAppointmentResult scheduleAppointmentResult = new()
            {
                Id = "test-id",
                DateTime = new DateTime(2022, 07, 21),
                DurationInMinutes = 30,
                TimeZone = "test-time-Zone",
                ScheduledTime = new DateTime(2022, 07, 21, 09, 00, 00)
            };

            serviceMock
                .Setup(x => x.ScheduleImagingAppointmentForPatientAsync(appointmentSchedulingRequest))
                .Returns(Task.FromResult(scheduleAppointmentResult));

            return serviceMock.Object;
        }
    }
}