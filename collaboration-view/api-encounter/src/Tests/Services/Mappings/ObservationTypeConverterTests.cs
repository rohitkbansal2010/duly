// <copyright file="ObservationTypeConverterTests.cs" company="Duly Health and Care">
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
    internal class ObservationTypeConverterTests : MapperConfigurator<RepositoryModelsToProcessContractsProfile>
    {
        [TestCase(VitalsCardType.BloodPressure, new[] { ObservationType.BloodPressure })]
        [TestCase(VitalsCardType.BloodOxygen, new[] { ObservationType.OxygenSaturation })]
        [TestCase(VitalsCardType.HeartRate, new[] { ObservationType.HeartRate })]
        [TestCase(VitalsCardType.RespiratoryRate, new[] { ObservationType.RespiratoryRate })]
        [TestCase(VitalsCardType.PainLevel, new[] { ObservationType.PainLevel })]
        [TestCase(VitalsCardType.Temperature, new[] { ObservationType.BodyTemperature })]
        [TestCase(VitalsCardType.WeightAndHeight, new[] { ObservationType.BodyWeight })]
        [TestCase(VitalsCardType.BodyMassIndex, new[] { ObservationType.BodyMassIndex })]
        public void ConvertTest(VitalsCardType sourceVitalsCardType, ObservationType[] targetObservationTypes)
        {
            //Arrange

            //Act
            var result = Mapper.Map<ObservationType[]>(sourceVitalsCardType);

            //Assert
            result.Length.Should().Be(targetObservationTypes.Length);
            for (var i = 0; i < result.Length; i++)
            {
                result[i].Should().Be(targetObservationTypes[i]);
            }
        }

        [Test]
        public void ConvertTest_Throw_ServiceNotMappedException()
        {
            //Arrange
            var source = (VitalsCardType)(-1);

            //Act
            Action action = () => Mapper.Map<ObservationType[]>(source);

            //Assert
            var result = action.Should().ThrowExactly<ServiceNotMappedException>();
            result.Which.Message.Should().Be("Could not map VitalsCardType to ObservationType");
        }
    }
}
