// <copyright file="ScheduleFollowuUpControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Testing;
using Duly.Ngdp.Api.Controllers;
using Duly.Ngdp.Api.Repositories.Interfaces;
using Duly.Ngdp.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Tests.Controllers
{
    [TestFixture]
    public class ScheduleFollowUpControllerTests
    {
        private Mock<IScheduleFollowUpRepository> _scheduleFollowUpRepositoryMocked;
        private Mock<ILogger<ScheduleFollowUpController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _scheduleFollowUpRepositoryMocked = new Mock<IScheduleFollowUpRepository>();
            _loggerMocked = new Mock<ILogger<ScheduleFollowUpController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public async Task PostScheduleFollowUpAsync_Test()
        {
            var scheduleFollowUp = new ScheduleFollowUp
            {
                Provider_ID = "Test-ProvideId",
                Patient_ID = "test-patient-id",
                AptType = "test-aptType",
                AptFormat = "test-aptFormat",
                Location_ID = "Test-LocationId",
                AptScheduleDate = new DateTime(2019, 05, 09, 9, 15, 0),
                BookingSlot = "test-bookingSlot",
                RefVisitType = "test-refVisitType",
                Created_Date = new DateTime(2019, 05, 09, 9, 15, 0),
                Type = "S",
                Appointment_Id = "test-appointmentId",
                Skipped = false
            };
            var controller = new ScheduleFollowUpController( _loggerMocked.Object, _iWebHostEnvironment.Object, _scheduleFollowUpRepositoryMocked.Object);
            controller.MockObjectValidator();

            var scheduleFollowUp1 = SetupRepository(scheduleFollowUp);

            var actionResult = await controller.PostScheduleFollowUp(scheduleFollowUp);

            actionResult.Result.Should().BeOfType<OkObjectResult>();

            var responseData = ((OkObjectResult)actionResult.Result).Value;
            responseData.Should().BeEquivalentTo(scheduleFollowUp1);
        }

        private int SetupRepository(ScheduleFollowUp scheduleFollowUp)
        {
            int res = 2;
            _scheduleFollowUpRepositoryMocked
                .Setup(repository =>
                    repository.PostScheduleFollowUpAsync(scheduleFollowUp))
                .ReturnsAsync(res);

            return res;
        }
    }
}
