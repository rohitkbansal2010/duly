// <copyright file="ImagingDetailsRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Implementations;
using Duly.Common.DataAccess.Contexts.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class ImagingDetailsRepositoryTests
    {
        private const string InsertImagingStoredProcedureName = "[InsertLabDetails]";

        private Mock<IDapperContext> _dapperContextMock;

        [SetUp]
        public void SetUp()
        {
            _dapperContextMock = new Mock<IDapperContext>();
        }

        [Test]
        public async Task PostImagingDetailsAsyncTest()
        {
            //Arrange
            var imagingDetails = new Models.CheckOut.ImagingDetails
            {
                ImagingType = "L",
                AptScheduleDate = new DateTime(2008, 5, 1, 8, 6, 32),
                Appointment_ID = "test-appointment-id",
                Patient_ID = "test-patient-id",
                Provider_ID = "test-provider-id",
                Location_ID = "test-location-id",
                BookingSlot = "test-Booking-slot",
                ImagingLocation = "test-imaging-location",
                Skipped = false
            };

            var repository = new ImagingDetailRepository(_dapperContextMock.Object);
            var labDetails1 = SetupDapperContext(imagingDetails);

            //Act
            var result = await repository.PostImagingDetailAsync(imagingDetails);

            //Assert
            Assert.AreEqual(result, labDetails1);
        }

        private int SetupDapperContext(Models.CheckOut.ImagingDetails imaging)
        {
            int res = 2;

            _dapperContextMock
                .Setup(context =>
                    context.ExecuteScalarAsync<int>(InsertImagingStoredProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(res);

            return res;
        }
    }
}
