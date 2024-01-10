// <copyright file="ServiceRequestControllerTests.cs" company="Duly Health and Care">
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
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Controllers
{
    [TestFixture]
    [SetCulture("en-us")]
    public class ServiceRequestControllerTests
    {
        private const string PatientId = "test-patient-id";
        private const string AppointmentId = "test-appointment-id";
        private const string Type = "test-type";

        private Mock<ILogger<ServiceRequestController>> _loggerMock;
        private Mock<IServiceRequestService> _serviceMock;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;
        private ServiceRequestController _controller;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<ServiceRequestController>>();
            _serviceMock = new Mock<IServiceRequestService>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();

            _controller = new ServiceRequestController(_serviceMock.Object, _loggerMock.Object, _iWebHostEnvironment.Object);
        }

        [Test]
        public void GetLabOrImagingOrdersTest()
        {
            //Arrange
            _ = SetupGetLabOrImagingOrdersAsync();

            ActionResult<ServiceRequest> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.GetLabOrImagingOrders(PatientId, AppointmentId, Type);
            act.Invoke();

            //Assert
            act.Should()
                .NotThrowAsync();

            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult.Value as ServiceRequest;
            contentResult.Should().NotBeNull();
        }

        private ServiceRequest SetupGetLabOrImagingOrdersAsync()
        {
            List<Orders> TestOrder1 = new()
            {
                    new()
                    {
                        OrderName = "Test-order-name",
                        AuthoredOn = "Test-authored-on"
                    }
            };
            var response = new ServiceRequest
            {
                OrderCount = 2,
                TestOrder = TestOrder1
            };

            _serviceMock
                .Setup(x => x.GetLabOrImagingOrdersAsync(PatientId, AppointmentId, Type))
                .Returns(Task.FromResult(response));

            return response;
        }
    }
}