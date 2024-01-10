// <copyright file="VitalsCardTypeConverterTests.cs" company="Duly Health and Care">
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
    internal class VitalsCardTypeConverterTests : MapperConfigurator<RepositoryModelsToProcessContractsProfile>
    {
        [TestCase(ObservationType.BloodPressure, VitalsCardType.BloodPressure)]
        [TestCase(ObservationType.BodyMassIndex, VitalsCardType.BodyMassIndex)]
        [TestCase(ObservationType.BodyTemperature, VitalsCardType.Temperature)]
        [TestCase(ObservationType.OxygenSaturation, VitalsCardType.BloodOxygen)]
        [TestCase(ObservationType.HeartRate, VitalsCardType.HeartRate)]
        [TestCase(ObservationType.RespiratoryRate, VitalsCardType.RespiratoryRate)]
        [TestCase(ObservationType.PainLevel, VitalsCardType.PainLevel)]
        [TestCase(ObservationType.BodyWeight, VitalsCardType.WeightAndHeight)]
        [TestCase(ObservationType.BodyHeight, VitalsCardType.WeightAndHeight)]
        public void ConvertTest(ObservationType sourceObservationType, VitalsCardType targetVitalsCardType)
        {
            //Arrange

            //Act
            var result = Mapper.Map<VitalsCardType>(sourceObservationType);

            //Assert
            result.Should().Be(targetVitalsCardType);
        }

        [Test]
        public void ConvertTest_Throw_ServiceNotMappedException()
        {
            //Arrange
            var source = (ObservationType)(-1);

            //Act
            Action action = () => Mapper.Map<VitalsCardType>(source);

            //Assert
            var result = action.Should().ThrowExactly<ServiceNotMappedException>();
            result.Which.Message.Should().Be("Could not map ObservationType to VitalsCardType");
        }
    }
}
