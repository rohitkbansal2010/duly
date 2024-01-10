// <copyright file="VitalTypeConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Mappings;
using Duly.CollaborationView.Encounter.Api.Tests.Common;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Mappings
{
    [TestFixture]
    internal class VitalTypeConverterTests : MapperConfigurator<RepositoryModelsToProcessContractsProfile>
    {
        [TestCase(ObservationType.BloodPressure, VitalType.BloodPressure)]
        [TestCase(ObservationType.BodyMassIndex, VitalType.BodyMassIndex)]
        [TestCase(ObservationType.BodyTemperature, VitalType.Temperature)]
        [TestCase(ObservationType.OxygenSaturation, VitalType.BloodOxygen)]
        [TestCase(ObservationType.HeartRate, VitalType.HeartRate)]
        [TestCase(ObservationType.RespiratoryRate, VitalType.RespiratoryRate)]
        [TestCase(ObservationType.PainLevel, VitalType.PainLevel)]
        [TestCase(ObservationType.BodyWeight, VitalType.Weight)]
        [TestCase(ObservationType.BodyHeight, VitalType.Height)]
        public void ConvertTest(ObservationType sourceObservationType, VitalType targetVitalType)
        {
            //Arrange

            //Act
            var result = Mapper.Map<VitalType>(sourceObservationType);

            //Assert
            result.Should().Be(targetVitalType);
        }

        [Test]
        public void ConvertTest_Throw_ServiceNotMappedException()
        {
            //Arrange
            var source = (ObservationType)(-1);

            //Act
            Action action = () => Mapper.Map<VitalType>(source);

            //Assert
            var result = action.Should().ThrowExactly<ServiceNotMappedException>();
            result.Which.Message.Should().Be("Could not map ObservationType to VitalType");
        }
    }
}
