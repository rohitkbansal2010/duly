// <copyright file="NgdpScheduleFollowUpRepositoryTests.cs" company="Duly Health and Care">
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
    public class NgdpScheduleFollowUpRepositoryTests
    {
        private Mock<IMapper> _mapperMocked;
        private Mock<IScheduleFollowUpAdapter> _adapterMocked;

        [SetUp]
        public void SetUp()
        {
            _adapterMocked = new Mock<IScheduleFollowUpAdapter>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]
        public async Task PostScheduleFollowUpAsync_Test()
        {
            var scheduleFollowUp = new Contracts.ScheduleFollowUp
            {
                Provider_ID = "Test-ProviderId",
                Patient_ID = "test-patient-id",
                AptType = "test-aptType",
                AptFormat = "test-aptFormat",
                Location_ID = "Test-LocationId",
                AptScheduleDate = new DateTime(2019, 05, 09, 9, 15, 0),
                BookingSlot = "test-bookingSlot",
                RefVisitType = "test-refVisitType",
                Created_Date = new DateTime(2019, 05, 09, 9, 15, 0),
                Type = "S",
                Appointment_Id = "test-appointmentId",
                Skipped = false
            };

            var scheduleFollowUpConverted = SetupMapper(scheduleFollowUp);
            var scheduleFollowUp1 = SetupAdapter(scheduleFollowUpConverted);

            var repository = new NgdpScheduleFollowUpRepository(_adapterMocked.Object, _mapperMocked.Object);

            //Act
            var result = await repository
                .PostScheduleFollowUpAsync(scheduleFollowUp);

            Assert.AreEqual(result, scheduleFollowUp1);
        }

        private int SetupAdapter(ScheduleFollowUp scheduleFollowUp)
        {
            int res = 2;

            _adapterMocked
                .Setup(adapter => adapter.PostScheduleFollowUpAsync(scheduleFollowUp))
                .ReturnsAsync(res);

            return res;
        }

        private AdapterModels.ScheduleFollowUp SetupMapper(Contracts.ScheduleFollowUp scheduleFollowUp)
        {
            AdapterModels.ScheduleFollowUp scheduleFollowUp1 = new();
            scheduleFollowUp1.Provider_ID = scheduleFollowUp.Provider_ID;
            scheduleFollowUp1.Patient_ID = scheduleFollowUp.Patient_ID;
            scheduleFollowUp1.AptType = scheduleFollowUp.AptType;
            scheduleFollowUp1.AptFormat = scheduleFollowUp.AptFormat;
            scheduleFollowUp1.Location_ID = scheduleFollowUp.Location_ID;
            scheduleFollowUp1.AptScheduleDate = scheduleFollowUp.AptScheduleDate;
            scheduleFollowUp1.BookingSlot = scheduleFollowUp.BookingSlot;
            scheduleFollowUp1.RefVisitType = scheduleFollowUp.RefVisitType;
            scheduleFollowUp1.Created_Date = scheduleFollowUp.Created_Date;
            scheduleFollowUp1.Type = scheduleFollowUp.Type;
            scheduleFollowUp1.Appointment_Id = scheduleFollowUp.Appointment_Id;
            scheduleFollowUp1.Skipped = scheduleFollowUp.Skipped;

            _mapperMocked
                .Setup(mapper => mapper.Map<AdapterModels.ScheduleFollowUp>(scheduleFollowUp))
                .Returns(scheduleFollowUp1);

            return scheduleFollowUp1;
        }
    }
}
