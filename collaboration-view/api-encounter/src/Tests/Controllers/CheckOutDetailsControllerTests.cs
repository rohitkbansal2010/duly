// <copyright file="CheckOutDetailsControllerTests.cs" company="Duly Health and Care">
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Controllers
{
    [TestFixture]
    [SetCulture("en-us")]
    public class CheckOutDetailsControllerTests
    {
        private const string AppointmentId = "test-appointmet-id";

        private Mock<ILogger<CheckOutDetailsController>> _loggerMock;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<CheckOutDetailsController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public void GetPatientByIdTest()
        {
            //Arrange
            ActionResult<CheckOutDetails> result = null;
            var controller = new CheckOutDetailsController(
                SetupCheckOutDetailsAsync(),
                _loggerMock.Object,
                _iWebHostEnvironment.Object);

            controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await controller.GetCheckOutDetails(AppointmentId);
            act.Invoke();

            //Assert
            act.Should()
                .NotThrowAsync();

            var okResult = (ObjectResult)result.Result;
            var contentResult = okResult.Value;

            contentResult.Should().NotBeNull();
        }

        private static ICheckOutDetailsservice SetupCheckOutDetailsAsync()
        {
            var serviceMock = new Mock<ICheckOutDetailsservice>();
            var result = new CheckOutDetails();
            var laborImagingList = new List<GetLabOrImaging>();
            var scheduleReferralList = new List<ScheduleReferral>();
            var labsorImaging = new GetLabOrImaging()
            {
                ID = 1,
                Type = "labs",
                Lab_ID = "test-lab",
                Lab_Location = "test-lab-location",
                Lab_Name = "test-lab-name",
                Appointment_ID = AppointmentId,
                Patient_ID = "test-patient-id",
                Provider_ID = " test-provider-id",
                Location_ID = "test-location-id",
                BookingSlot = "test-booking-slot",
                ImagingLocation = "test-imaging-location",
                ImagingType = "test-imaging-type",
                Skipped = false,
                AptScheduleDate = new DateTime(2008, 5, 1, 8, 6, 32)
            };

            var scheduleReferral = new ScheduleReferral()
            {
                ID = 1,
                Patient_ID = "test-patient-id",
                Provider_ID = "test-provider-id",
                AptType = "test-appointment",
                AptFormat = "test-aptFormat",
                Location_ID = "test-location-id",
                AptScheduleDate = new DateTime(2008, 5, 1, 8, 6, 32),
                BookingSlot = "test-slot",
                RefVisitType = "test-visit",
                Type = "R",
                Appointment_Id = AppointmentId,
                Skipped = false,
                Department_Id = "test-department",
                VisitTypeId = "test-visit"
            };

            laborImagingList.Add(labsorImaging);
            scheduleReferralList.Add(scheduleReferral);

            result.LabDetailsList = laborImagingList;
            result.ScheduleFollowUpList = scheduleReferralList;

            serviceMock
                .Setup(x => x.GetCheckOutDetailsAsync(AppointmentId))
                .Returns(Task.FromResult(result));

            return serviceMock.Object;
        }
    }
}