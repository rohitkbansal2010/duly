// <copyright file="ServiceRequestRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Implementations;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class ServiceRequestRepositoryTests
    {
        private const string PatientId = "test-patient-id";
        private const string AppointmentId = "test-appointment-id";
        private const string Type = "test-type";

        private Mock<IEncounterContext> _encounterContextMock;
        private Mock<IAppointmentIdClient> _clientMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            ConfigureEncounterContext();
            _clientMock = new Mock<Clinic.Api.Client.IAppointmentIdClient>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task GetLabsOrImagingAsyncSuccessTest()
        {

            List<Orders> TestOrder1 = new()
            {
                new()
                {
                    OrderName = "Test-order-name",
                    AuthoredOn = "Test-authored-on"
                }
            };
            var response1 = new ServiceRequest
            {
                OrderCount = 2,
                TestOrder = TestOrder1
            };

            List<Models.Orders> TestOrder2 = new()
            {
                new()
                {
                    OrderName = "Test-order-name",
                    AuthoredOn = "Test-authored-on"
                }
            };
            var response2 = new Models.ServiceRequest
            {
                OrderCount = 2,
                TestOrder = TestOrder2
            };

            _clientMock
                .Setup(client => client.TypeAsync(PatientId, AppointmentId, Type, It.IsAny<Guid>(), default))
                .Returns(Task.FromResult(response1));

            _mapperMock
                .Setup(mapper => mapper.Map<List<Models.Orders>>(TestOrder1))
                .Returns(TestOrder2);

            IServiceRequestRepository repositoryMock = new ServiceRequestRepository(
                _encounterContextMock.Object,
                _mapperMock.Object,
                _clientMock.Object);

            //Act
            var results = await repositoryMock.GetLabsOrImagingAsync(PatientId, AppointmentId, Type);

            //Assert
            _clientMock.Verify(x => x.TypeAsync(PatientId, AppointmentId, Type, It.IsAny<Guid>(), default), Times.Once());

            results.Should().NotBeNull();
            results.Should().BeOfType<Models.ServiceRequest>();
        }

        private void ConfigureEncounterContext()
        {
            _encounterContextMock = new Mock<IEncounterContext>();

            _encounterContextMock
                .Setup(ctx => ctx.GetXCorrelationId())
                .Returns(Guid.NewGuid());
        }
    }
}
