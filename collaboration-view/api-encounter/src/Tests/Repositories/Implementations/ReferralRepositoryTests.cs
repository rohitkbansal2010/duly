// <copyright file="ReferralRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Implementations;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Api.Client;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class ReferralRepositoryTests
    {
        private const string PostReferralDetailStoredProcedureName = "[insertScheduleFollowUp]";
        private const string InsertDataPostedToEpicStoredProcedureName = "[UpdateDataPostedToEpic]";

        private Mock<IDapperContext> _dapperContextMock;

        [SetUp]
        public void SetUp()
        {
            _dapperContextMock = new Mock<IDapperContext>();
        }

        [Test]
        public async Task PostReferralTest()
        {
            var referralDetail = new Models.CheckOut.ScheduleReferral
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

            var refDetails = SetupDapperContext(referralDetail);

            IReferralRepository repositoryMock = new ReferralRepository(
                _dapperContextMock.Object);

            var results = await repositoryMock.PostReferral(referralDetail);

            //Assert
            Assert.AreEqual(results, refDetails);
        }

        [Test]
        public async Task PostDataPostedToEpicTest()
        {
            int id = 1;

            _ = SetupPostDataPostedToEpicDapperContext();

            var repositoryMock = new ReferralRepository(_dapperContextMock.Object);

            var results = await repositoryMock.PostDataPostedToEpic(id);

            //Assert
            Assert.AreEqual(results, id);
        }

        private int SetupDapperContext(Models.CheckOut.ScheduleReferral referral)
        {
            int res = 2;

            _dapperContextMock
                .Setup(context =>
                    context.ExecuteScalarAsync<int>(PostReferralDetailStoredProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(res);

            return res;
        }

        private int SetupPostDataPostedToEpicDapperContext()
        {
            int res = 2;

            _dapperContextMock
                .Setup(context =>
                    context.ExecuteScalarAsync<int>(InsertDataPostedToEpicStoredProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(res);

            return res;
        }
    }
}
