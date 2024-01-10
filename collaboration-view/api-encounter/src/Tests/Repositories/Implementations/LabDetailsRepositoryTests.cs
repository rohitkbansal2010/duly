// <copyright file="LabDetailsRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Implementations;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.Ngdp.Api.Client;
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
    public class LabDetailsRepositoryTests
    {
        private Mock<IEncounterContext> _encounterContextMock;
        private Mock<IClient> _clientMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILatlngClient> _latlngClient;

        [SetUp]
        public void SetUp()
        {
            ConfigureEncounterContext();
            _clientMock = new Mock<IClient>();
            _mapperMock = new Mock<IMapper>();
            _latlngClient = new Mock<ILatlngClient>();
        }

        [Test]
        public async Task PostLabDetailsAsyncTest()
        {
            //Arrange
            //const string patientId = "123";
            //var epicPatient = BuildEpicPatient(patientId);
            var labDetails = new Models.CheckOut.LabDetails
            {
                Type = "L",
                Lab_ID = "test-lab-id",
                Lab_Location = "test-lab-location",
                Lab_Name = "test-lab-name",
                CreatedDate = new System.DateTime(2008, 5, 1, 8, 6, 32),
                Appointment_ID = "test-appointment-id",
                Patient_ID = "test-patient-id",
                Skipped = false
            };

            var labDetails1 = new LabDetails
            {
                Type = labDetails.Type,
                Lab_ID = labDetails.Lab_ID,
                Lab_Location = labDetails.Lab_Location,
                Lab_Name = labDetails.Lab_Name,
                CreatedDate = labDetails.CreatedDate,
                Appointment_ID = labDetails.Appointment_ID,
                Patient_ID = labDetails.Patient_ID,
                Skipped = labDetails.Skipped
            };

            //ConfigureMapper(epicPatient);
            var res = 2;

            _clientMock
                .Setup(client => client.LabDetailsAsync(It.IsAny<Guid>(), labDetails1, default))
                .Returns(Task.FromResult(res));

            _mapperMock
                .Setup(mapper => mapper.Map<LabDetails>(labDetails))
                .Returns(labDetails1);

            ILabDetailRepository repositoryMock = new LabDetailRepository(
                _encounterContextMock.Object,
                _mapperMock.Object,
                _clientMock.Object,
                _latlngClient.Object);

            //Act
            var results = await repositoryMock.PostLabDetailAsync(labDetails);

            //Assert
            _clientMock.Verify(x => x.LabDetailsAsync(It.IsAny<Guid>(), labDetails1, default), Times.Once());

            results.Should().NotBe(0);
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
