// <copyright file="LoincObservationCodesConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Repositories.Mappings.ObservationConverters;
using Duly.Clinic.Contracts;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings.ObservationConverters
{
    [TestFixture]
    public class LoincObservationCodesConverterTests
    {
        [TestCase("85354-9", ObservationType.BloodPressure)]
        [TestCase("2708-6", ObservationType.OxygenSaturation)]
        [TestCase("8867-4", ObservationType.HeartRate)]
        [TestCase("9279-1", ObservationType.RespiratoryRate)]
        [TestCase("72514-3", ObservationType.PainLevel)]
        [TestCase("8310-5", ObservationType.BodyTemperature)]
        [TestCase("29463-7", ObservationType.BodyWeight)]
        [TestCase("8302-2", ObservationType.BodyHeight)]
        [TestCase("39156-5", ObservationType.BodyMassIndex)]
        public void ConvertFromLoincCodeToObservationTypeTest(string source, ObservationType? expected)
        {
            //Act
            var result = source.ConvertFromLoincCodeToObservationType(out var detectedType);

            //Assert
            result.Should().BeTrue();
            detectedType.Should().Be(expected);
        }

        [Test]
        public void ConvertFromLoincCodeToObservationTypeTest_Default()
        {
            //Arrange
            var str = "default_code";

            //Act
            var result = str.ConvertFromLoincCodeToObservationType(out var detectedType);

            //Assert
            result.Should().BeFalse();
            detectedType.Should().BeNull();
        }

        [TestCase(ObservationType.BloodPressure, "85354-9")]
        [TestCase(ObservationType.HeartRate, "8867-4")]
        [TestCase(ObservationType.RespiratoryRate, "9279-1")]
        [TestCase(ObservationType.PainLevel, "72514-3")]
        [TestCase(ObservationType.BodyTemperature, "8310-5")]
        [TestCase(ObservationType.BodyWeight, "29463-7")]
        [TestCase(ObservationType.BodyHeight, "8302-2")]
        [TestCase(ObservationType.BodyMassIndex, "39156-5")]
        public void ConvertFromObservationTypeToLoincCodeTest(ObservationType source, string expected)
        {
            //Act
            var result = source.ConvertFromObservationTypeToLoincCode(out var loincCodes);

            //Assert
            result.Should().BeTrue();
            loincCodes.Should().HaveCount(1);
            loincCodes.First().Should().Be(expected);
        }

        [Test]
        public void ConvertFromObservationTypeToLoincCodeTest_OxygenSaturation()
        {
            //Arrange
            var observationType = ObservationType.OxygenSaturation;

            //Act
            var result = observationType.ConvertFromObservationTypeToLoincCode(out var loincCodes);

            //Assert
            result.Should().BeTrue();
            loincCodes.Should().HaveCount(2);
            loincCodes.First().Should().Be("2708-6");
            loincCodes.Last().Should().Be("59408-5");
        }

        [Test]
        public void ConvertFromObservationTypeToLoincCodeTest_Default()
        {
            //Arrange
            var observationType = (ObservationType)(-1);

            //Act
            var result = observationType.ConvertFromObservationTypeToLoincCode(out var loincCodes);

            //Assert
            result.Should().BeFalse();
            loincCodes.Should().BeNull();
        }
    }
}
