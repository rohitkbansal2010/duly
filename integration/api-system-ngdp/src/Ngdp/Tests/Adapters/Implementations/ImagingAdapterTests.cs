// <copyright file="ImagingAdapterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Adapter;
using Duly.Ngdp.Adapter.Adapters.Implementations;
using Duly.Ngdp.Adapter.Adapters.Models;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ngdp.Adapter.Tests.Adapters.Implementations
{
    [TestFixture]
    public class ImagingAdapterTests
    {
        private const string PostImagingProcedureName = "[InsertLabDetails]";

        private Mock<ICVDapperContext> _mockedDapperContext;

        [SetUp]
        public void Setup()
        {
            _mockedDapperContext = new Mock<ICVDapperContext>();
        }

        [Test]
        public async Task PostImagingAsync_Test()
        {
            //Arrange
            //const int referralId = 123456677;

            var Imaging = new ImagingDetail
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
            var adapter = new ImagingAdapter(_mockedDapperContext.Object);
            var labDetails1 = SetupDapperContext(Imaging);

            //Act
            var result = await adapter.ImagingAsync(Imaging);

            //Assert
            Assert.AreEqual(result, labDetails1);
        }

        private int SetupDapperContext(ImagingDetail imaging)
        {
            int res = 2;

            _mockedDapperContext
                .Setup(context =>
                    context.ExecuteScalarAsync<int>(PostImagingProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(res);

            return res;
        }
    }
}
