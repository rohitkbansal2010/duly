// <copyright file="TimeSlotConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Api.Tests.Common;
using Duly.Clinic.Contracts;
using FluentAssertions;
using Hl7.Fhir.Model;
using NUnit.Framework;
using System;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class TimeSlotConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        [Test]
        public void ConvertTest()
        {
            //Arrange
            var timeSpan = new TimeSpan(2, 0, 0);
            var start = new DateTimeOffset(2012, 11, 21, 1, 14, 15, timeSpan);
            var end = new DateTimeOffset(2012, 11, 21, 3, 14, 15, timeSpan);

            Hl7.Fhir.Model.Period source = new()
            {
                StartElement = new FhirDateTime(start),
                EndElement = new FhirDateTime(end)
            };

            //Act
            var result = Mapper.Map<TimeSlot>(source);

            //Assert
            result.Should().NotBeNull();
            result.StartTime.Should().Be(start);
            result.EndTime.Should().Be(end);
        }
    }
}
