// <copyright file="LabDetailsControllerTests.cs" company="Duly Health and Care">
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
    public class LabDetailsControllerTests
    {
        private Mock<ILabDetailsRepository> _repositoryMocked;
        private Mock<ILogger<LabDetailsController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _repositoryMocked = new Mock<ILabDetailsRepository>();
            _loggerMocked = new Mock<ILogger<LabDetailsController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public async Task PostLabDetailsTest()
        {
            //Arrange
            var labDetails = new LabDetails
            {
                Type = "L",
                Lab_ID = "test-lab-id",
                Lab_Location = "test-lab-location",
                Lab_Name = "test-lab-name",
                CreatedDate = new DateTime(2008, 5, 1, 8, 6, 32),
                Appointment_ID = "test-appointment-id",
                Patient_ID = "test-patient-id",
                Skipped = false
            };

            var controller = new LabDetailsController(_repositoryMocked.Object, _loggerMocked.Object, _iWebHostEnvironment.Object);
            controller.MockObjectValidator();

            var labDetails1 = SetupRepository(labDetails);

            //Act
            var actionResult = await controller.PostLabDetails(labDetails);

            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = ((OkObjectResult)actionResult.Result).Value;
            responseData.Should().BeEquivalentTo(labDetails1);
        }

        private int SetupRepository(LabDetails labDetails)
        {
            int res = 2;
            _repositoryMocked
                .Setup(repository =>
                    repository.PostLabDetailsAsync(labDetails))
                .ReturnsAsync(res);

            return res;
        }
    }
}