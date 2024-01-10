// <copyright file="GetSlotsControllerTests.cs" company="Duly Health and Care">
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
    public class GetSlotsControllerTests
    {
        private static ImagingTimeSlot _objImagingSlot = null;
        private Mock<ILogger<GetSlotsController>> _loggerMock;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;
        private Departments _objdepartments = null;
        private List<Departments> _listDepart;
        private List<string> _liststring;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<GetSlotsController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
            _objdepartments = new Departments();
            _liststring = new List<string>();
            _liststring.Add("3940");
            _liststring.Add("6003");
            _liststring.Add("6004");

            _objdepartments.DepartmentId = "20876";
            _objdepartments.ProviderId = _liststring;
            _listDepart = new List<Departments>();
            _listDepart.Add(_objdepartments);

            _objImagingSlot = new ImagingTimeSlot
            {
                DepartmentAndProvider = _listDepart,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2)
            };
        }

        [Test]
        public void GetImagingTimeSlotsTest()
        {
            //Arrange
            ActionResult<List<ImagingScheduleDate>> result = null;
            var controller = new GetSlotsController(
                SetupGetImagingScheduleAsync(),
                _loggerMock.Object,
                _iWebHostEnvironment.Object);

            controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await controller.ImagingTimeSlots(_objImagingSlot);
            act.Invoke();

            //Assert
            act.Should()
                .NotThrowAsync();

            var okResult = (ObjectResult)result.Result;
            var contentResult = okResult.Value;

            contentResult.Should().NotBeNull();
        }

        private static IGetSlotsservice SetupGetImagingScheduleAsync()
        {
            var serviceMock = new Mock<IGetSlotsservice>();

            List<TimeSlots> timeSlot = new List<TimeSlots>
            {
                   new TimeSlots
                {
                    DisplayTime = new System.TimeSpan(09, 00, 00),
                    Time = new System.TimeSpan(09, 00, 00),
                }
            };

            List<Duly.CollaborationView.Encounter.Api.Contracts.ScheduleDate> scheduleDate = new List<Duly.CollaborationView.Encounter.Api.Contracts.ScheduleDate>
            {
              new Duly.CollaborationView.Encounter.Api.Contracts.ScheduleDate
                {
                    Date = new System.DateTime(2022, 07, 21),
                    TimeSlots = timeSlot
                }
            };

            List<ImagingScheduleDate> result = new List<ImagingScheduleDate>
            {
              new ImagingScheduleDate { ProviderId = "3940", ScheduleDates = scheduleDate }
            };

            serviceMock
                .Setup(x => x.GetImagingScheduleAsync(_objImagingSlot))
                .Returns(Task.FromResult(result));

            return serviceMock.Object;
        }

        [Test]
        public void GetOpenTimeSlotsForProviderTest()
        {
            //Arrange
            string visitTypeId = "4576";
            string appointmentId = "12345";
            DateTime startDate = new DateTime(2022, 07, 22);
            DateTime endDate = new DateTime(2022, 07, 23);

            ActionResult<IEnumerable<ScheduleDate>> result = null;
            var controller = new GetSlotsController(
                SetupGetScheduleDateAsync(visitTypeId, appointmentId, startDate, endDate),
                _loggerMock.Object,
                _iWebHostEnvironment.Object);

            controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await controller.GetOpenTimeSlotsForProvider(visitTypeId, appointmentId, startDate, endDate);
            act.Invoke();

            //Assert
            act.Should()
                .NotThrowAsync();

            var okResult = (ObjectResult)result.Result;
            var contentResult = okResult.Value;

            contentResult.Should().NotBeNull();
        }

        private static IGetSlotsservice SetupGetScheduleDateAsync(string visitTypeId, string appointmentId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var serviceMock = new Mock<IGetSlotsservice>();
            IEnumerable<Duly.CollaborationView.Encounter.Api.Contracts.ScheduleDate> _result = new Duly.CollaborationView.Encounter.Api.Contracts.ScheduleDate[]
            {
              new Duly.CollaborationView.Encounter.Api.Contracts.ScheduleDate
                {
                    Date = new System.DateTime(2022, 07, 21)
                }
            };

            serviceMock
                .Setup(x => x.GetScheduleDateAsync(visitTypeId, appointmentId, startDate, endDate))
                .Returns(Task.FromResult(_result));

            return serviceMock.Object;
        }

        [Test]
        public void GetOpenReferralTimeSlotsForProviderTest()
        {
            //Arrange
            string visitTypeId = "4576";
            string departmentId = "20876";
            string providerId = "3940";
            string appointmentId = "12345";
            DateTime startDate = new DateTime(2022, 07, 22);
            DateTime endDate = new DateTime(2022, 07, 23);

            ActionResult<IEnumerable<ScheduleDate>> result = null;
            var controller = new GetSlotsController(
                SetupGetReferralScheduleDateAsync(visitTypeId, departmentId, providerId, startDate, endDate),
                _loggerMock.Object,
                _iWebHostEnvironment.Object);

            controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await controller.GetOpenTimeSlotsForProvider(visitTypeId, appointmentId, startDate, endDate);
            act.Invoke();

            //Assert
            act.Should()
                .NotThrowAsync();

            var okResult = (ObjectResult)result.Result;
            var contentResult = okResult.Value;

            contentResult.Should().NotBeNull();
        }

        private static IGetSlotsservice SetupGetReferralScheduleDateAsync(string visitTypeId, string departmentId, string providerId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var serviceMock = new Mock<IGetSlotsservice>();
            IEnumerable<Duly.CollaborationView.Encounter.Api.Contracts.ScheduleDate> _result = new Duly.CollaborationView.Encounter.Api.Contracts.ScheduleDate[]
            {
              new Duly.CollaborationView.Encounter.Api.Contracts.ScheduleDate
                {
                    Date = new System.DateTime(2022, 07, 21)
                }
            };

            serviceMock
                .Setup(x => x.GetReferralScheduleDateAsync(visitTypeId, departmentId, providerId, startDate, endDate))
                .Returns(Task.FromResult(_result));

            return serviceMock.Object;
        }
    }
}