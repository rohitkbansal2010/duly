// <copyright file="LabDetailsControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Controllers;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Controllers
{
    [TestFixture]
    [SetCulture("en-us")]
    public class LabDetailsControllerTests
    {
        //private const string PatientId = "test-patient-id";
        private const int CvcheckOutId = 2;
        private static LabDetail _labDetail = new LabDetail
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

        private Mock<ILogger<LabDetailController>> _loggerMock;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<LabDetailController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public void GetPatientByIdTest()
        {
            //Arrange
            ActionResult<CreationResultResponse> result = null;
            var controller = new LabDetailController(
                SetupPostLabDetailAsync(),
                _loggerMock.Object,
                _iWebHostEnvironment.Object);

            controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await controller.PostLabDetail(_labDetail);
            act.Invoke();

            //Assert
            act.Should()
                .NotThrowAsync();

            var okResult = (ObjectResult)result.Result;
            var contentResult = okResult.Value;

            contentResult.Should().NotBeNull();
        }

        private static ILabDetailService SetupPostLabDetailAsync()
        {
            var serviceMock = new Mock<ILabDetailService>();

            serviceMock
                .Setup(x => x.PostLabDetailAsync(_labDetail))
                .Returns(Task.FromResult(CvcheckOutId));

            return serviceMock.Object;
        }
    }
}