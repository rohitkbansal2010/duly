// <copyright file="LabDetailsServiceTests.cs" company="Duly Health and Care">
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
using System.Threading.Tasks;
using ApiContracts = Duly.CollaborationView.Encounter.Api.Contracts;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations
{
    [TestFixture]
    public class LabDetailsServiceTests
    {
        private Mock<ILabDetailRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<ILabDetailRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task PostLabDetailsAsyncTest()
        {
            //Arrange
            //const string patientId = "123";
            //var patient = BuildPatient(patientId);
            var labDetails = new LabDetail
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

            int res = 2;

            _repositoryMock
                .Setup(repo => repo.PostLabDetailAsync(labDetails1))
                .Returns(Task.FromResult(res));

            _mapperMock
                .Setup(mapper => mapper.Map<LabDetails>(labDetails))
                .Returns(labDetails1);

            var serviceMock = new LabDetailService(
                _mapperMock.Object,
                _repositoryMock.Object);

            //Act
            var results = await serviceMock.PostLabDetailAsync(labDetails);

            //Assert
            _repositoryMock.Verify(x => x.PostLabDetailAsync(labDetails1), Times.Once());

            results.Should().NotBe(0);
        }
    }
}