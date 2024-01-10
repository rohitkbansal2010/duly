// <copyright file="ServiceRequestServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using Duly.CollaborationView.Encounter.Api.Services.Implementations;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiContracts = Duly.CollaborationView.Encounter.Api.Contracts;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations
{
    [TestFixture]
    public class ServiceRequestServiceTests
    {
        private const string PatientId = "test-patient-id";
        private const string AppointmentId = "test-appointment-id";
        private const string Type = "test-type";

        private Mock<IServiceRequestRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IServiceRequestRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task GetLabOrImagingOrdersAsyncSuccessTest()
        {
            List<Api.Repositories.Models.Orders> TestOrder1 = new()
            {
                new()
                {
                    OrderName = "Test-order-name",
                    AuthoredOn = "Test-authored-on"
                }
            };
            var response1 = new Api.Repositories.Models.ServiceRequest
            {
                OrderCount = 2,
                TestOrder = TestOrder1
            };

            List<ApiContracts.Orders> TestOrder2 = new()
            {
                new()
                {
                    OrderName = "Test-order-name",
                    AuthoredOn = "Test-authored-on"
                }
            };
            var response2 = new ApiContracts.ServiceRequest
            {
                OrderCount = 2,
                TestOrder = TestOrder2
            };

            _repositoryMock
                .Setup(repo => repo.GetLabsOrImagingAsync(PatientId, AppointmentId, Type))
                .Returns(Task.FromResult(response1));

            _mapperMock
                .Setup(mapper => mapper.Map<List<ApiContracts.Orders>>(TestOrder1))
                .Returns(TestOrder2);

            var serviceMock = new ServiceRequestService(
                _mapperMock.Object,
                _repositoryMock.Object);

            //Act
            var results = await serviceMock.GetLabOrImagingOrdersAsync(PatientId, AppointmentId, Type);

            //Assert
            _repositoryMock.Verify(x => x.GetLabsOrImagingAsync(PatientId, AppointmentId, Type), Times.Once());

            results.Should().NotBeNull();
        }
    }
}