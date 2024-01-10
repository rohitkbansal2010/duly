// <copyright file="NgdpReferralDetailRepositoryTests.cs" company="Duly Health and Care">
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
    public class NgdpReferraldetailRepositoryTests
    {
        private Mock<IMapper> _mapperMocked;
        private Mock<IReferralDetailAdapter> _adapterMocked;

        [SetUp]
        public void SetUp()
        {
            _adapterMocked = new Mock<IReferralDetailAdapter>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]
        public async Task PostReferralDetailAsync_Test()
        {
            var referralDetail = new Contracts.ReferralDetail
            {
                Provider_ID = "Tet-ProviderID",
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

            var referralDetailConverted = SetupMapper(referralDetail);
            var referralDetail1 = SetupAdapter(referralDetailConverted);

            var repository = new NgdpReferralDetailRepository(_adapterMocked.Object, _mapperMocked.Object);

            //Act
            var result = await repository
                .PostReferralDetailAsync(referralDetail);

            Assert.AreEqual(result, referralDetail1);
        }

        private int SetupAdapter(ReferralDetail referralDetail)
        {
            int res = 2;

            _adapterMocked
                .Setup(adapter => adapter.PostReferralDetailAsync(referralDetail))
                .ReturnsAsync(res);

            return res;
        }

        private AdapterModels.ReferralDetail SetupMapper(Contracts.ReferralDetail referralDetail)
        {
            AdapterModels.ReferralDetail referralDetail1 = new();
            referralDetail1.Provider_ID = referralDetail.Provider_ID;
            referralDetail1.Patient_ID = referralDetail.Patient_ID;
            referralDetail1.AptType = referralDetail.AptType;
            referralDetail1.AptFormat = referralDetail.AptFormat;
            referralDetail1.Location_ID = referralDetail.Location_ID;
            referralDetail1.AptScheduleDate = referralDetail.AptScheduleDate;
            referralDetail1.BookingSlot = referralDetail.BookingSlot;
            referralDetail1.RefVisitType = referralDetail.RefVisitType;
            referralDetail1.Created_Date = referralDetail.Created_Date;
            referralDetail1.Type = referralDetail.Type;
            referralDetail1.Appointment_Id = referralDetail.Appointment_Id;
            referralDetail1.Skipped = referralDetail.Skipped;

            _mapperMocked
                .Setup(mapper => mapper.Map<AdapterModels.ReferralDetail>(referralDetail))
                .Returns(referralDetail1);

            return referralDetail1;
        }
    }
}
