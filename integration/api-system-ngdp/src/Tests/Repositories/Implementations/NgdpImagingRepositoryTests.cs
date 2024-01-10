// <copyright file="NgdpImagingRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Models;
using Duly.Ngdp.Api.Repositories.Implementations;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class NgdpImagingRepositoryTests
    {
        private Mock<IMapper> _mapperMocked;
        private Mock<IImagingAdapter> _adapterMocked;

        [SetUp]
        public void SetUp()
        {
            _adapterMocked = new Mock<IImagingAdapter>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]
        public async Task PostImagingAsync_Test()
        {
            //Arrange
            var Imaging = new Contracts.ImagingDetail
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

            var imagingConverted = SetupMapper(Imaging);
            var imaging1 = SetupAdapter(imagingConverted);

            var repository = new NgdpImagingRepository(_adapterMocked.Object, _mapperMocked.Object);

            //Act
            var result = await repository
                .PostImagingAsync(Imaging);

            //Assert
            Assert.AreEqual(result, imaging1);
        }

        private int SetupAdapter(ImagingDetail imaging)
        {
            int res = 2;

            _adapterMocked
                .Setup(adapter => adapter.ImagingAsync(imaging))
                .ReturnsAsync(res);

            return res;
        }

        private AdapterModels.ImagingDetail SetupMapper(Contracts.ImagingDetail imaging)
        {
            AdapterModels.ImagingDetail imaging1 = new();
            imaging1.ImagingType = imaging.ImagingType;
            imaging1.Appointment_ID = imaging.Appointment_ID;
            imaging1.Patient_ID = imaging.Patient_ID;
            imaging1.Provider_ID = imaging.Provider_ID;
            imaging1.Location_ID = imaging.Location_ID;
            imaging1.BookingSlot = imaging.BookingSlot;
            imaging1.AptScheduleDate = imaging.AptScheduleDate;
            imaging1.ImagingLocation = imaging.ImagingLocation;
            imaging1.Skipped = imaging.Skipped;

            _mapperMocked
                .Setup(mapper => mapper.Map<AdapterModels.ImagingDetail>(imaging))
                .Returns(imaging1);

            return imaging1;
        }
    }
}
