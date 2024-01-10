// <copyright file="ReferralAppointmentConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Api.Configurations;
using Duly.Ngdp.Api.Repositories.Mappings;
using Duly.Ngdp.Api.Tests.Common;
using Duly.Ngdp.Contracts;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

namespace Duly.Ngdp.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class ReferralAppointmentConverterTests : MapperConfigurator<NgdpToSystemApiContractsProfile>
    {
        protected override object ConstructService(Type serviceType)
        {
            if (serviceType == typeof(ReferralAppointmentConverter))
            {
                return new ReferralAppointmentConverter();
            }

            throw new InvalidOperationException($"Can not create instance of type {serviceType}");
        }

        [Test]
        public void Convert_ProviderName_Test()
        {
            //Arrange
            var source = new Adapter.Adapters.Models.ReferralAppointment
            {
                ProviderDisplayName = "Syed Hasan, MD",
            };

            //Act
            var result = Mapper.Map<ReferralAppointment>(source);

            //Assert
            result.Should().NotBeNull();
            result.Provider.Name.Should().Be(source.ProviderDisplayName);
        }

        [Test]
        public void Convert_DepartmentName_Test()
        {
            //Arrange
            var source = new Adapter.Adapters.Models.ReferralAppointment
            {
                DepartmentName = "Cardiology - Airlite St, Elgin",
            };

            //Act
            var result = Mapper.Map<ReferralAppointment>(source);

            //Assert
            result.Should().NotBeNull();
            result.Department.Name.Should().Be(source.DepartmentName);
        }

        [Test]
        public void Convert_VisitType_Test()
        {
            //Arrange
            var source = new Adapter.Adapters.Models.ReferralAppointment
            {
                VisitTypeExternalId = "5507",
            };

            //Act
            var result = Mapper.Map<ReferralAppointment>(source);

            //Assert
            result.Should().NotBeNull();
            result.Visit.Type.Identifier.Id.Should().Be(source.VisitTypeExternalId);
            result.Visit.Type.Identifier.Type.Should().Be(IdentifierType.External);
        }

        [Test]
        public void Convert_AppointmentDateTime_Test()
        {
            //Arrange
            var source = new Adapter.Adapters.Models.ReferralAppointment
            {
                AppointmentDate = DateTime.Now.AddDays(5),
                AppointmentTime = TimeSpan.FromHours(15),
                AppointmentDurationInMins = 15
            };

            //Act
            var result = Mapper.Map<ReferralAppointment>(source);

            //Assert
            result.Should().NotBeNull();
            result.Appointment.DateTime.Should().Be(source.AppointmentDate.GetValueOrDefault().Date.Add(
                source.AppointmentTime.GetValueOrDefault()));
            result.Appointment.DurationInMinutes.Should().Be(source.AppointmentDurationInMins);
        }

        [Test]
        public void Convert_ScheduledTime_Test()
        {
            //Arrange
            var source = new Adapter.Adapters.Models.ReferralAppointment
            {
                AppointmentScheduledTime = DateTime.Now.AddDays(5),
            };

            //Act
            var result = Mapper.Map<ReferralAppointment>(source);

            //Assert
            result.Should().NotBeNull();
            result.ScheduledTime.Should().Be(source.AppointmentScheduledTime.Value.DateTime);
        }

        [Test]
        public void Convert_Location_Test()
        {
            //Arrange
            var source = new Adapter.Adapters.Models.ReferralAppointment
            {
                DepartmentCity = "ELGIN",
                DepartmentState = "IL",
                DepartmentStreet = "87 N AIRLITE ST"
            };

            //Act
            var result = Mapper.Map<ReferralAppointment>(source);

            //Assert
            result.Should().NotBeNull();
            result.Location.Address.City.Should().Be(source.DepartmentCity);
            result.Location.Address.State.Should().Be(source.DepartmentState);
            result.Location.Address.Lines.First().Should().Be(source.DepartmentStreet);
        }
    }
}
