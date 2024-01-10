// <copyright file="ChartDatasetLabelBuilderTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Implementations.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations.Extensions
{
    [TestFixture]
    internal class ChartDatasetLabelBuilderTests
    {
        [TestCase(ObservationType.BodyMassIndex, "BMI")]
        [TestCase(ObservationType.BodyHeight, "Height")]
        [TestCase(ObservationType.BodyWeight, "Weight")]
        [TestCase(ObservationType.BodyTemperature, "Temperature")]
        [TestCase(ObservationType.PainLevel, "Pain Level")]
        [TestCase(ObservationType.RespiratoryRate, "Respiratory Rate")]
        [TestCase(ObservationType.HeartRate, "Heart Rate")]
        [TestCase(ObservationType.OxygenSaturation, "Blood Oxygen")]
        public void BuildChartDatasetLabel_Test(ObservationType observationType, string expectedLabel)
        {
            //Arrange

            //Act
            var result = observationType.BuildChartDatasetLabel();

            //Assert
            result.Should().Be(expectedLabel);
        }

        [Test]
        public void BuildChartDatasetLabel_Throw_ArgumentOutOfRangeException()
        {
            //Arrange
            var source = (ObservationType)(-1);

            //Act
            Action action = () => source.BuildChartDatasetLabel();

            //Assert
            var result = action.Should().ThrowExactly<ArgumentOutOfRangeException>();
            result.Which.Message.Should().Be(
                $"Exception of type 'System.ArgumentOutOfRangeException' was thrown. (Parameter 'observationType'){Environment.NewLine}Actual value was -1.");
        }

        [TestCase(ObservationComponentType.Diastolic, "Diastolic")]
        [TestCase(ObservationComponentType.Systolic, "Systolic")]
        public void BuildChartDatasetLabel_Test(ObservationComponentType observationComponentType, string expectedLabel)
        {
            //Arrange

            //Act
            var result = observationComponentType.BuildChartDatasetLabel();

            //Assert
            result.Should().Be(expectedLabel);
        }

        [Test]
        public void BuildChartDatasetLabel_ObservationComponentType_Throw_ArgumentOutOfRangeException()
        {
            //Arrange
            var source = (ObservationComponentType)(-1);

            //Act
            Action action = () => source.BuildChartDatasetLabel();

            //Assert
            var result = action.Should().ThrowExactly<ArgumentOutOfRangeException>();
            result.Which.Message.Should().Be(
                $"Exception of type 'System.ArgumentOutOfRangeException' was thrown. (Parameter 'observationComponentType'){Environment.NewLine}Actual value was -1.");
        }
    }
}
