// <copyright file="ScheduleFollowupServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Services.Implementations;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using ApiContracts = Duly.CollaborationView.Encounter.Api.Contracts;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations
{
    [TestFixture]
    public class ScheduleFollowupServiceTests
    {
        private Mock<IScheduleFollowupRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IScheduleFollowupRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task PostScheduleFollowupTest()
        {
            //Arrange
            var scheduleFollowup = new ScheduleFollowUp
            {
                Provider_ID = "Test-Provider-id",
                Patient_ID = "test-patient-id",
                AptType = "test-aptType",
                AptFormat = "test-aptFormat",
                Location_ID = "Test-Location-id",
                AptScheduleDate = new DateTime(2019, 05, 09, 9, 15, 0),
                BookingSlot = "test-bookingSlot",
                RefVisitType = "test-refVisitType",
                Created_Date = new DateTime(2019, 05, 09, 9, 15, 0),
                Type = "S",
                Appointment_Id = "test-appointmentId",
                Skipped = false
            };

            var scheduleFollowup1 = new Models.ScheduleFollowUp
            {
                Provider_ID = "Test-Provider-id",
                Patient_ID = "test-patient-id",
                AptType = "test-aptType",
                AptFormat = "test-aptFormat",
                Location_ID = "Test-Location-id",
                AptScheduleDate = new DateTime(2019, 05, 09, 9, 15, 0),
                BookingSlot = "test-bookingSlot",
                RefVisitType = "test-refVisitType",
                Created_Date = new DateTime(2019, 05, 09, 9, 15, 0),
                Type = "S",
                Appointment_Id = "test-appointmentId",
                Skipped = false
            };

            int res = 2;

            _repositoryMock
                .Setup(repo => repo.PostScheduleFollowup(scheduleFollowup1))
                .Returns(Task.FromResult(res));

            _mapperMock
                .Setup(mapper => mapper.Map<Models.ScheduleFollowUp>(scheduleFollowup))
                .Returns(scheduleFollowup1);

            var serviceMock = new ScheduleFollowupService(
                _mapperMock.Object,
                _repositoryMock.Object);

            var results = await serviceMock.PostScheduleFollowup(scheduleFollowup);

            _repositoryMock.Verify(x => x.PostScheduleFollowup(scheduleFollowup1), Times.Once());

            results.Should().NotBe(0);
        }

        [Test]
        public async Task DataPostedToEpicAsync()
        {
            //Arrange
            int id = 1;
            int res = 1;

            _repositoryMock
                .Setup(repo => repo.PostDataPostedToEpic(id))
                .Returns(Task.FromResult(res));

            var serviceMock = new ScheduleFollowupService(
                _mapperMock.Object,
                _repositoryMock.Object);

            var results = await serviceMock.DataPostedToEpicAsync(id);

            _repositoryMock.Verify(x => x.PostDataPostedToEpic(id), Times.Once());

            results.Should().NotBe(0);
        }
    }
}