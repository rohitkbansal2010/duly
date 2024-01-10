// <copyright file="ChartDatasetVisibilityIdentifierTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Implementations.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations.Extensions
{
    [TestFixture]
    internal class ChartDatasetVisibilityIdentifierTests
    {
        [TestCase(ObservationType.RespiratoryRate, true)]
        [TestCase(ObservationType.PainLevel, true)]
        [TestCase(ObservationType.OxygenSaturation, true)]
        [TestCase(ObservationType.BodyWeight, true)]
        [TestCase(ObservationType.BodyTemperature, true)]
        [TestCase(ObservationType.BodyMassIndex, true)]
        [TestCase(ObservationType.HeartRate, true)]
        [TestCase(ObservationType.BloodPressure, true)]
        [TestCase(ObservationType.BodyHeight, false)]
        public void BuildVisibleTest(ObservationType observationType, bool expectedResult)
        {
            //Arrange

            //Act
            var result = observationType.BuildVisible();

            //Assert
            result.Should().Be(expectedResult);
        }
    }
}
