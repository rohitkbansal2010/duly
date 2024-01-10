// <copyright file="ReferralDetailControllerTests.cs" company="Duly Health and Care">
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
    public class ReferralControllerTests
    {
        private Mock<IReferralDetailRepository> _referralDetailRepositoryMocked;
        private Mock<ILogger<ReferralDetailController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _referralDetailRepositoryMocked = new Mock<IReferralDetailRepository>();
            _loggerMocked = new Mock<ILogger<ReferralDetailController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public async Task PostReferralDetailAsync_Test()
        {
            var referralDetail = new ReferralDetail
            {
                Provider_ID = "Test-ProviderId",
                Patient_ID = "test-patient-id",
                AptType = "test-aptType",
                AptFormat = "test-aptFormat",
                Location_ID = "Test-Location-Id",
                AptScheduleDate = new DateTime(2001, 04, 09, 9, 15, 0),
                BookingSlot = "test-bookingSlot",
                RefVisitType = "test-refVisitType",
                Created_Date = new DateTime(2000, 07, 09, 9, 15, 0),
                Type = "S",
                Appointment_Id = "test-appointmentId",
                Skipped = false
            };
            var controller = new ReferralDetailController( _loggerMocked.Object, _iWebHostEnvironment.Object, _referralDetailRepositoryMocked.Object);
            controller.MockObjectValidator();

            var referralDetail1 = SetupRepository(referralDetail);

            var actionResult = await controller.PostReferralDetail(referralDetail);

            actionResult.Result.Should().BeOfType<OkObjectResult>();

            var responseData = ((OkObjectResult)actionResult.Result).Value;
            responseData.Should().BeEquivalentTo(referralDetail1);
        }

        private int SetupRepository(ReferralDetail referralDetail)
        {
            int res = 2;
            _referralDetailRepositoryMocked
                .Setup(repository =>
                    repository.PostReferralDetailAsync(referralDetail))
                .ReturnsAsync(res);

            return res;
        }
    }
}
