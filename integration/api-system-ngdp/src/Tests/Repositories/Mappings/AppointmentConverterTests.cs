// <copyright file="AppointmentConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Api.Configurations;
using Duly.Ngdp.Api.Repositories.Mappings;
using Duly.Ngdp.Api.Repositories.Mappings.Converters;
using Duly.Ngdp.Api.Tests.Common;
using Duly.Ngdp.Contracts;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;

namespace Duly.Ngdp.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class AppointmentConverterTests : MapperConfigurator<NgdpToSystemApiContractsProfile>
    {
        protected override object ConstructService(Type serviceType)
        {
            if (serviceType == typeof(AppointmentConverter))
            {
                var mockTimeZoneConverter = new Mock<ITimeZoneConverter>();

                mockTimeZoneConverter
                    .Setup(tz => tz.ToCstDateTime(It.IsAny<DateTimeOffset>()))
                    .Returns<DateTimeOffset>(dt => dt.DateTime);

                mockTimeZoneConverter
                    .Setup(tz => tz.ToCstDateTimeOffset(It.IsAny<DateTime>()))
                    .Returns<DateTime>(dt => new DateTimeOffset(dt, TimeSpan.Zero));

                return new AppointmentConverter(mockTimeZoneConverter.Object);
            }

            throw new InvalidOperationException($"Can not create instance of type {serviceType}");
        }

        [Test]
        public void Convert_ProviderName_Test()
        {
            //Arrange
            var source = new Adapter.Adapters.Models.Appointment
            {
                ProviderName = "test"
            };

            //Act
            var result = Mapper.Map<Appointment>(source);

            //Assert

            result.Should().NotBeNull();
            result.Practitioner.HumanName.FamilyName.Should().Be(source.ProviderName);
            result.Status.Should().Be(AppointmentStatus.Unknown);
        }

        [Test]
        public void Convert_CsnId_Test()
        {
            //Arrange
            var source = new Adapter.Adapters.Models.Appointment
            {
                ProviderName = "test",
                CsnId = decimal.MinusOne
            };

            //Act
            var result = Mapper.Map<Appointment>(source);

            //Assert

            result.Should().NotBeNull();
            result.Id.Should().Be(source.CsnId.ToString());
        }

        [Test]
        public void Convert_ProviderName_Split_Test()
        {
            //Arrange
            var familyName = "TestF";
            var givenName = "ATest";

            var source = new Adapter.Adapters.Models.Appointment
            {
                ProviderName = $"{familyName}, {givenName}"
            };

            //Act
            var result = Mapper.Map<Appointment>(source);

            //Assert

            result.Should().NotBeNull();
            result.Practitioner.HumanName.FamilyName.Should().Be(familyName);
            result.Practitioner.HumanName.GivenNames.Length.Should().Be(1);
            result.Practitioner.HumanName.GivenNames[0].Should().Be(givenName);
        }

        [TestCase("Arrived", AppointmentStatus.Arrived)]
        [TestCase("Canceled", AppointmentStatus.Canceled)]
        [TestCase("Completed", AppointmentStatus.Completed)]
        [TestCase("Left without seen", AppointmentStatus.LeftWithoutSeen)]
        [TestCase("Charge Entered", AppointmentStatus.ChargeEntered)]
        [TestCase("No Show", AppointmentStatus.NoShow)]
        [TestCase("Scheduled", AppointmentStatus.Scheduled)]
        [TestCase("Unresolved", AppointmentStatus.Unresolved)]
        public void Convert_StatusName_Test(string statusName, AppointmentStatus targetStatus)
        {
            //Arrange
            var source = new Adapter.Adapters.Models.Appointment
            {
                ProviderName = "test",
                StatusName = statusName
            };

            //Act
            var result = Mapper.Map<Appointment>(source);

            //Assert

            result.Should().NotBeNull();
            result.Practitioner.HumanName.FamilyName.Should().Be(source.ProviderName);
            result.Status.Should().Be(targetStatus);
        }

        [Test]
        public void Convert_Note_Test()
        {
            //Arrange
            var source = new Adapter.Adapters.Models.Appointment
            {
                ProviderName = "test",
                Note = "Reason"
            };

            //Act
            var result = Mapper.Map<Appointment>(source);

            //Assert
            result.Should().NotBeNull();
            result.Note.Should().Be(source.Note);
        }

        [TestCase("Y", true)]
        [TestCase("y", true)]
        [TestCase("N", false)]
        [TestCase("n", false)]
        public void Convert_IsTelehealthVisit_Test(string isTelehealthVisit, bool isTelehealthVisitConverted)
        {
            //Arrange
            var source = new Adapter.Adapters.Models.Appointment
            {
                ProviderName = "test",
                IsTelehealthVisit = isTelehealthVisit
            };

            //Act
            var result = Mapper.Map<Appointment>(source);

            //Assert
            result.Should().NotBeNull();
            result.IsTelehealthVisit.Should().Be(isTelehealthVisitConverted);
        }

        [TestCase("Y", true)]
        [TestCase("y", true)]
        [TestCase("N", false)]
        [TestCase("n", false)]
        [TestCase("anything_else", false)]
        public void Convert_IsProtectedByBtg_Test(string isProtectedByBtg, bool isProtectedByBtgConverted)
        {
            //Arrange
            var source = new Adapter.Adapters.Models.Appointment
            {
                ProviderName = "test",
                IsUnderBtg = isProtectedByBtg
            };

            //Act
            var result = Mapper.Map<Appointment>(source);

            //Assert
            result.Should().NotBeNull();
            result.IsProtectedByBtg.Should().Be(isProtectedByBtgConverted);
        }
    }
}
