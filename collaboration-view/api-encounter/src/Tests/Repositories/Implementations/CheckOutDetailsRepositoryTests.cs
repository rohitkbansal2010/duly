// <copyright file="CheckOutDetailsRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Implementations;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Api.Client;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class CheckOutDetailsRepositoryTests
    {
        private const string FindLabDetailsProcedureName = "[uspGetLabDetails]";
        private const string FindScheduleFollowUpProcedureName = "[uspGetScheduleFollowUp]";
        private Mock<IDapperContext> _dapperContextMock;

        [SetUp]
        public void SetUp()
        {
            _dapperContextMock = new Mock<IDapperContext>();
        }

        [Test]
        public async Task PostReferralTest()
        {
            const string appointmentId = "test-appointment-id";

            var refDetails = SetupDapperContext(appointmentId);

            ICheckOutDetailsRepository repositoryMock = new CheckOutDetailsRepository(
                _dapperContextMock.Object);

            var results = await repositoryMock.GetCheckOutDetailsByIdAsync(appointmentId);

            //Assert
            Assert.NotNull(results);
            Assert.NotNull(refDetails);
        }

        private Models.CheckOut.CheckOutDetails SetupDapperContext(string appointmentId)
        {
            var result = new Models.CheckOut.CheckOutDetails();
            var laborImagingList = new List<GetLabOrImaging>();
            var scheduleReferralList = new List<ScheduleReferral>();
            var labsorImaging = new GetLabOrImaging()
            {
                ID = 1,
                Type = "labs",
                Lab_ID = "test-lab",
                Lab_Location = "test-lab-location",
                Lab_Name = "test-lab-name",
                Appointment_ID = "appointment-id",
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
                Appointment_Id = "appointment-id",
                Skipped = false,
                Department_Id = "test-department"
            };

            laborImagingList.Add(labsorImaging);
            scheduleReferralList.Add(scheduleReferral);

            result.LabDetailsList = laborImagingList;
            result.ScheduleFollowUpList = scheduleReferralList;

            _dapperContextMock
                .Setup(context =>
                    context.QueryAsync<Models.CheckOut.GetLabOrImaging>(FindLabDetailsProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(laborImagingList);

            _dapperContextMock
                .Setup(context =>
                    context.QueryAsync<Models.CheckOut.ScheduleReferral>(FindScheduleFollowUpProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(scheduleReferralList);

            return result;
        }
    }
}
