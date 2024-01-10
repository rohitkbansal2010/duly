// <copyright file="ImmunizationConverterTests.cs" company="Duly Health and Care">
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
    public class ImmunizationConverterTests : MapperConfigurator<NgdpToSystemApiContractsProfile>
    {
        protected override object ConstructService(Type serviceType)
        {
            if (serviceType == typeof(ImmunizationConverter))
            {
                var mockTimeZoneConverter = new Mock<ITimeZoneConverter>();

                mockTimeZoneConverter
                    .Setup(tz => tz.ToCstDateTime(It.IsAny<DateTimeOffset>()))
                    .Returns<DateTimeOffset>(dt => dt.DateTime);

                mockTimeZoneConverter
                    .Setup(tz => tz.ToCstDateTimeOffset(It.IsAny<DateTime>()))
                    .Returns<DateTime>(dt => new DateTimeOffset(dt, TimeSpan.Zero));

                return new ImmunizationConverter(mockTimeZoneConverter.Object);
            }

            throw new InvalidOperationException($"Can not create instance of type {serviceType}");
        }

        [Test]
        public void Convert_VaccineName_Test()
        {
            //Arrange
            var source = new Adapter.Adapters.Models.Immunization
            {
                VaccineName = "test"
            };

            //Act
            var result = Mapper.Map<Immunization>(source);

            //Assert

            result.Should().NotBeNull();
            result.VaccineName.Should().Be(source.VaccineName);
        }

        [Test]
        public void Convert_DueDate_Test()
        {
            //Arrange
            var source = new Adapter.Adapters.Models.Immunization
            {
                DueDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified)
            };

            //Act
            var result = Mapper.Map<Immunization>(source);

            //Assert

            result.Should().NotBeNull();
            result.DueDate.Should().Be(new DateTimeOffset(source.DueDate, TimeSpan.Zero));
        }

        [Test]
        public void Convert_Patient_Test()
        {
            //Arrange
            var source = new Adapter.Adapters.Models.Immunization
            {
                PatientExternalId = "12"
            };

            //Act
            var result = Mapper.Map<Immunization>(source);

            //Assert

            result.Should().NotBeNull();
            result.Patient.Id.Should().Be(source.PatientExternalId);
        }

        [TestCase(1, DueStatus.NotDue)]
        [TestCase(2, DueStatus.DueSoon)]
        [TestCase(3, DueStatus.DueOn)]
        [TestCase(4, DueStatus.Overdue)]
        [TestCase(5, DueStatus.Postponed)]
        public void Convert_Status_Test(int statusId, DueStatus expectedDueStatus)
        {
            //Arrange
            var source = new Adapter.Adapters.Models.Immunization
            {
                StatusId = statusId
            };

            //Act
            var result = Mapper.Map<Immunization>(source);

            //Assert

            result.Should().NotBeNull();
            result.Status.Should().Be(expectedDueStatus);
        }
    }
}