// <copyright file="SlotsControllerTests.cs" company="Duly Health and Care">
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
    public class SlotsControllerTests
    {
        private Mock<IScheduleRepository> _scheduleRepositoryMocked;
        private Mock<ILogger<SlotsController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _scheduleRepositoryMocked = new Mock<IScheduleRepository>();
            _loggerMocked = new Mock<ILogger<SlotsController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public async Task GetSlots_Test()
        {
            //Arrange
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddHours(5);
            var providerId = "external|101";
            var departmentId = "unknown|202";
            var visitTypeId = "external|303";

            var schedules = ConfigureRepository(startDate, endDate, providerId, departmentId, visitTypeId);
            var slotsController = new SlotsController(_scheduleRepositoryMocked.Object, _loggerMocked.Object, _iWebHostEnvironment.Object);

            slotsController.MockObjectValidator();

            //Act
            var actionResult = await slotsController.GetSlots(startDate, endDate, providerId, departmentId, visitTypeId);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<ScheduleDay>)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().NotBeNull();
            responseData.First().Date.Should().Be(schedules.First().Date);
            responseData.First().Slots.First().ArrivalTime.Should().Be(schedules.First().Slots.First().ArrivalTime);
            responseData.First().Slots.First().DisplayTime.Should().Be(schedules.First().Slots.First().DisplayTime);
            responseData.First().Slots.First().Time.Should().Be(schedules.First().Slots.First().Time);
        }

        private IEnumerable<ScheduleDay> ConfigureRepository(DateTime startDate, DateTime endDate, string providerId, string departmentId, string visitTypeId)
        {
            IEnumerable<ScheduleDay> schedules = new ScheduleDay[]
            {
                new()
                {
                    Date = DateTimeOffset.UtcNow,
                    Slots = new Slot[]
                    {
                        new()
                        {
                            ArrivalTime = TimeSpan.FromHours(3),
                            DisplayTime = TimeSpan.FromHours(4),
                            Time = TimeSpan.FromHours(5),
                        }
                    }
                }
            };

            _scheduleRepositoryMocked
                .Setup(repository => repository.GetScheduleAsync(startDate, endDate, providerId, departmentId, visitTypeId))
                .ReturnsAsync(schedules);

            return schedules;
        }
    }
}
