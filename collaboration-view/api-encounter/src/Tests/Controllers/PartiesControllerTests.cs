// <copyright file="PartiesControllerTests.cs" company="Duly Health and Care">
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Controllers
{
    [TestFixture]
    public class PartiesControllerTests
    {
        private const string TestAppointmentId = "test-appointment-id";
        private const string TestPatientId = "test-patient-id";

        private PartiesController _controller;

        [SetUp]
        public void SetUp()
        {
            var loggerMock = new Mock<ILogger<PartiesController>>();
            var appointmentServiceMock = new Mock<IPartyService>();
            appointmentServiceMock.Setup(x => x.GetPartiesByPatientAndAppointmentIdAsync(TestPatientId, TestAppointmentId))
                .ReturnsAsync(new List<Party> { new() { Id = TestAppointmentId } });
            var iWebHostEnvironment = new Mock<IWebHostEnvironment>();
            _controller = new PartiesController(appointmentServiceMock.Object, loggerMock.Object, iWebHostEnvironment.Object);

            _controller.MockObjectValidator();
        }

        [Test]
        public async Task GetPartiesByAppointmentIdAsync_Test()
        {
            //Arrange

            //Act
            var result = await _controller.GetPartiesByPatientAndAppointmentId(TestPatientId, TestAppointmentId);

            //Assert
            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult.Value as IEnumerable<Party>;
            contentResult.Should().NotBeNullOrEmpty();
            contentResult.FirstOrDefault().Id.Should().Be(TestAppointmentId, "This is the one that is created in setup");
        }
    }
}
