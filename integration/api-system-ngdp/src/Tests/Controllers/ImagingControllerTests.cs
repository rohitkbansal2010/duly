// <copyright file="ImagingControllerTests.cs" company="Duly Health and Care">
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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Tests.Controllers
{
    [TestFixture]
    public class ImagingControllerTests
    {
        private Mock<IImagingRepository> _repositoryMocked;
        private Mock<ILogger<ImagingController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _repositoryMocked = new Mock<IImagingRepository>();
            _loggerMocked = new Mock<ILogger<ImagingController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public async Task GetImmunizationsForSpecificPatientTest()
        {
            //Arrange
            var Imaging = new ImagingDetail
            {
                ImagingType = "L",
                AptScheduleDate = new DateTime(2008, 5, 1, 8, 6, 32),
                Appointment_ID = "test-appointment-id",
                Patient_ID = "test-patient-id",
                Provider_ID = "test-provider-id",
                Location_ID = "test-location-id",
                BookingSlot = "test-Booking-slot",
                ImagingLocation = "test-imaging-location",
                Skipped = false
            };

            var controller = new ImagingController(_repositoryMocked.Object, _loggerMocked.Object, _iWebHostEnvironment.Object);
            controller.MockObjectValidator();

            var Imaging1 = SetupRepository(Imaging);

            //Act
            var actionResult = await controller.PostImaging(Imaging);

            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = ((OkObjectResult)actionResult.Result).Value;
            responseData.Should().BeEquivalentTo(Imaging1);
        }

        private int SetupRepository(ImagingDetail imaging)
        {
            int res = 2;
            _repositoryMocked
                .Setup(repository =>
                    repository.PostImagingAsync(imaging))
                .ReturnsAsync(res);

            return res;
        }
    }
}