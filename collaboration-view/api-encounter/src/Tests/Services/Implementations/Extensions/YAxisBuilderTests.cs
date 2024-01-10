// <copyright file="YAxisBuilderTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Services.Implementations.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations.Extensions
{
    [TestFixture]
    public class YAxisBuilderTests
    {
        [TestCase(VitalsCardType.BodyMassIndex, null, 83, 13, 14)]
        [TestCase(VitalsCardType.WeightAndHeight, null, 210, 30, 30)]
        [TestCase(VitalsCardType.WeightAndHeight, VitalPreferredUnitOfMeasure.PreferredWeightUnitOfMeasure, 480, 80, 50)]
        [TestCase(VitalsCardType.PainLevel, null, 10, 1, 2)]
        [TestCase(VitalsCardType.RespiratoryRate, null, 60, 0, 10)]
        [TestCase(VitalsCardType.Temperature, null, 42, 34, 2)]
        [TestCase(VitalsCardType.Temperature, VitalPreferredUnitOfMeasure.PreferredTemperatureUnitOfMeasure, 120, 90, 5)]
        [TestCase(VitalsCardType.HeartRate, null, 200, 0, 40)]
        [TestCase(VitalsCardType.BloodOxygen, null, 100, 80, 4)]
        [TestCase(VitalsCardType.BloodPressure, null, 240, 0, 40)]
        public void BuildYAxisTest(VitalsCardType vitalsCardType, string unitOfMeasure, decimal expectedMax, decimal expectedMin, decimal expectedStepSize)
        {
            //Arrange

            //Act
            var result = vitalsCardType.BuildYAxis(unitOfMeasure);

            //Assert
            result.Should().NotBeNull();
            result.Max.Should().Be(expectedMax);
            result.Min.Should().Be(expectedMin);
            result.StepSize.Should().Be(expectedStepSize);
        }

        [Test]
        public void ConvertTest_Throw_ServiceNotMappedException()
        {
            //Arrange
            var source = (VitalsCardType)(-1);

            //Act
            Action action = () => source.BuildYAxis(null);

            //Assert
            var result = action.Should().ThrowExactly<ArgumentOutOfRangeException>();
            result.Which.Message.Should().Be(
                $"Exception of type 'System.ArgumentOutOfRangeException' was thrown. (Parameter 'vitalsCardType'){Environment.NewLine}Actual value was -1.");
        }
    }
}
