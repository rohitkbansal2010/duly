// <copyright file="ImagingDetailsServiceTests.cs" company="Duly Health and Care">
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
    public class ImagingDetailsServiceTests
    {
        private Mock<IImagingDetailRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IImagingDetailRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task PostImagingDetailsAsyncTest()
        {
            //Arrange
            //const string patientId = "123";
            //var patient = BuildPatient(patientId);
            var imagingDetails = new ImagingDetail
            {
                ImagingType = "I",
                Appointment_ID = "test-appointment-id",
                Patient_ID = "test-patient-id",
                Provider_ID = "test-provider-id",
                Location_ID = "test-location-id",
                BookingSlot = "test-booking-slot",
                AptScheduleDate = new System.DateTime(2008, 5, 1, 8, 6, 32),
                ImagingLocation = "test-imaging-location",
                Skipped = false
            };

            var imagingDetails1 = new ImagingDetails
            {
                ImagingType = imagingDetails.ImagingType,
                Appointment_ID = imagingDetails.Appointment_ID,
                Patient_ID = imagingDetails.Patient_ID,
                Provider_ID = imagingDetails.Provider_ID,
                Location_ID = imagingDetails.Location_ID,
                BookingSlot = imagingDetails.BookingSlot,
                AptScheduleDate = imagingDetails.AptScheduleDate,
                ImagingLocation = imagingDetails.ImagingLocation,
                Skipped = imagingDetails.Skipped
            };

            int res = 2;

            _repositoryMock
                .Setup(repo => repo.PostImagingDetailAsync(imagingDetails1))
                .Returns(Task.FromResult(res));

            _mapperMock
                .Setup(mapper => mapper.Map<ImagingDetails>(imagingDetails))
                .Returns(imagingDetails1);

            var serviceMock = new ImagingDetailService(
                _mapperMock.Object,
                _repositoryMock.Object);

            //Act
            var results = await serviceMock.PostImagingDetailAsync(imagingDetails);

            //Assert
            _repositoryMock.Verify(x => x.PostImagingDetailAsync(imagingDetails1), Times.Once());

            results.Should().NotBe(0);
        }
    }
}