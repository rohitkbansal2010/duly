// <copyright file="ReferralDetailAdapterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Implementations;
using Duly.Ngdp.Adapter.Adapters.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Ngdp.Adapter.Tests.Adapters.Implementations
{
    [TestFixture]
    public class ReferraldetailAdapterTests
    {
        private const string PostReferralDetailProcedureName = "[insertScheduleFollowUp]";
        private Mock<ICVDapperContext> _mockedDapperContext;

        [SetUp]
        public void Setup()
        {
            _mockedDapperContext = new Mock<ICVDapperContext>();
        }

        [Test]
        public async Task PostReferralDetailAsync()
        {
            //Arrange
            var referralDetail = new ReferralDetail
            {
                Provider_ID = "Test-ProviderID",
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
            var adapter = new ReferralDetailAdapter(_mockedDapperContext.Object);
            var referralDetail1 = SetupDapperContext(referralDetail);

            //Act
            var result = await adapter.PostReferralDetailAsync(referralDetail);

            //Assert
            Assert.AreEqual(result, referralDetail1);
        }

        private int SetupDapperContext(ReferralDetail referralDetail)
        {
            int res = 2;

            _mockedDapperContext
                .Setup(context =>
                    context.ExecuteScalarAsync<int>(PostReferralDetailProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(res);

            return res;
        }
    }
}