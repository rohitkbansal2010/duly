// <copyright file="ReferralServiceTests.cs" company="Duly Health and Care">
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
    public class ReferralServiceTests
    {
        private Mock<IReferralRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IReferralRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task PostReferralTest()
        {
            //Arrange
            var referralDetail = new ScheduleReferral
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

            var referralDetail1 = new Models.ScheduleReferral
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
                .Setup(repo => repo.PostReferral(referralDetail1))
                .Returns(Task.FromResult(res));

            _mapperMock
                .Setup(mapper => mapper.Map<Models.ScheduleReferral>(referralDetail))
                .Returns(referralDetail1);

            var serviceMock = new ReferralService(
                _mapperMock.Object,
                _repositoryMock.Object);

            var results = await serviceMock.PostReferral(referralDetail);

            _repositoryMock.Verify(x => x.PostReferral(referralDetail1), Times.Once());

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

            var serviceMock = new ReferralService(
                _mapperMock.Object,
                _repositoryMock.Object);

            var results = await serviceMock.DataPostedToEpicAsync(id);

            _repositoryMock.Verify(x => x.PostDataPostedToEpic(id), Times.Once());

            results.Should().NotBe(0);
        }
    }
}