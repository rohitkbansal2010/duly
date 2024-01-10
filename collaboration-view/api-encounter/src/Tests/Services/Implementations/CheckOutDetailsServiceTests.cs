// <copyright file="CheckOutDetailsServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
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
    public class CheckOutDetailsServiceTests
    {
        private Mock<ICheckOutDetailsRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<ICheckOutDetailsRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task GetCheckOutDetailsAsyncTest()
        {
            //Arrange
            const string appointmentId = "123";
            var patient = BuildCheckOutDetails(appointmentId);
            ConfigureMapper(patient);
            var serviceMock = new CheckOutDetailsService(
                _mapperMock.Object,
                _repositoryMock.Object);

            //Act
            var results = await serviceMock.GetCheckOutDetailsAsync(appointmentId);

            //Assert
            _repositoryMock.Verify(x => x.GetCheckOutDetailsByIdAsync(appointmentId), Times.Once());

            results.Should().NotBeNull();
        }

        private CheckOutDetails BuildCheckOutDetails(string apointmentId)
        {
            var result = new CheckOutDetails();
            var laborImagingList = new List<GetLabOrImaging>();
            var scheduleReferralList = new List<ScheduleReferral>();
            var labsorImaging = new GetLabOrImaging()
            {
                ID = 1,
                Type = "labs",
                Lab_ID = "test-lab",
                Lab_Location = "test-lab-location",
                Lab_Name = "test-lab-name",
                Appointment_ID = apointmentId,
                Patient_ID = "test-patient-id",
                Provider_ID = " test-provider-id",
                Location_ID = "test-location-id",
                BookingSlot = "test-booking-slot",
                ImagingLocation = "test-imaging-location",
                ImagingType = "test-imaging-type",
                Skipped = false,
                AptScheduleDate = new DateTime(2008, 5, 1, 8, 6, 32)
            };

            var scheduleReferral = new ScheduleReferral()
            {
                ID = 1,
                Patient_ID = "test-patient-id",
                Provider_ID = "test-provider-id",
                AptType = "test-appointment",
                AptFormat = "test-aptFormat",
                Location_ID = "test-location-id",
                AptScheduleDate = new DateTime(2008, 5, 1, 8, 6, 32),
                BookingSlot = "test-slot",
                RefVisitType = "test-visit",
                Type = "R",
                Appointment_Id = apointmentId,
                Skipped = false,
                Department_Id = "test-department"
            };

            laborImagingList.Add(labsorImaging);
            scheduleReferralList.Add(scheduleReferral);

            result.LabDetailsList = laborImagingList;
            result.ScheduleFollowUpList = scheduleReferralList;

            _repositoryMock
                .Setup(repo => repo.GetCheckOutDetailsByIdAsync(apointmentId))
                .Returns(Task.FromResult(result));

            return result;
        }

        private void ConfigureMapper(CheckOutDetails checkOutDetails)
        {
            var result = new ApiContracts.CheckOutDetails();
            var laborImagingList = new List<ApiContracts.GetLabOrImaging>();
            var scheduleReferralList = new List<ApiContracts.ScheduleReferral>();
            var labsorImaging = new ApiContracts.GetLabOrImaging()
            {
                ID = 1,
                Type = "labs",
                Lab_ID = "test-lab",
                Lab_Location = "test-lab-location",
                Lab_Name = "test-lab-name",
                Appointment_ID = "test-appointment-id",
                Patient_ID = "test-patient-id",
                Provider_ID = " test-provider-id",
                Location_ID = "test-location-id",
                BookingSlot = "test-booking-slot",
                ImagingLocation = "test-imaging-location",
                ImagingType = "test-imaging-type",
                Skipped = false,
                AptScheduleDate = new DateTime(2008, 5, 1, 8, 6, 32)
            };

            var scheduleReferral = new ApiContracts.ScheduleReferral()
            {
                ID = 1,
                Patient_ID = "test-patient-id",
                Provider_ID = "test-provider-id",
                AptType = "test-appointment",
                AptFormat = "test-aptFormat",
                Location_ID = "test-location-id",
                AptScheduleDate = new DateTime(2008, 5, 1, 8, 6, 32),
                BookingSlot = "test-slot",
                RefVisitType = "test-visit",
                Type = "R",
                Appointment_Id = "test-appointment-id",
                Skipped = false,
                Department_Id = "test-department"
            };

            laborImagingList.Add(labsorImaging);
            scheduleReferralList.Add(scheduleReferral);

            result.LabDetailsList = laborImagingList;
            result.ScheduleFollowUpList = scheduleReferralList;

            _mapperMock
                .Setup(mapper => mapper.Map<ApiContracts.CheckOutDetails>(checkOutDetails))
                .Returns(result);
        }
    }
}