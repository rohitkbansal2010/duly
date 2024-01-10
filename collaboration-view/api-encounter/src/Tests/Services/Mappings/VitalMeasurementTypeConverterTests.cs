// <copyright file="VitalMeasurementTypeConverterTests.cs" company="Duly Health and Care">
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
    internal class VitalMeasurementTypeConverterTests : MapperConfigurator<RepositoryModelsToProcessContractsProfile>
    {
        [TestCase(ObservationComponentType.Diastolic, VitalMeasurementType.DiastolicBloodPressure)]
        [TestCase(ObservationComponentType.Systolic, VitalMeasurementType.SystolicBloodPressure)]
        public void ConvertTest(ObservationComponentType sourceObservationComponentType, VitalMeasurementType targetVitalMeasurementType)
        {
            //Arrange

            //Act
            var result = Mapper.Map<VitalMeasurementType>(sourceObservationComponentType);

            //Assert
            result.Should().Be(targetVitalMeasurementType);
        }

        [Test]
        public void ConvertTest_Throw_ServiceNotMappedException()
        {
            //Arrange
            var source = (ObservationComponentType)(-1);

            //Act
            Action action = () => Mapper.Map<VitalMeasurementType>(source);

            //Assert
            var result = action.Should().ThrowExactly<ServiceNotMappedException>();
            result.Which.Message.Should().Be("Could not map ObservationComponentType to VitalMeasurementType");
        }

        [TestCase(ObservationType.OxygenSaturation, VitalMeasurementType.OxygenSaturation)]
        [TestCase(ObservationType.HeartRate, VitalMeasurementType.HeartRate)]
        [TestCase(ObservationType.RespiratoryRate, VitalMeasurementType.RespiratoryRate)]
        [TestCase(ObservationType.PainLevel, VitalMeasurementType.PainLevel)]
        [TestCase(ObservationType.BodyTemperature, VitalMeasurementType.BodyTemperature)]
        [TestCase(ObservationType.BodyWeight, VitalMeasurementType.BodyWeight)]
        [TestCase(ObservationType.BodyHeight, VitalMeasurementType.BodyHeight)]
        [TestCase(ObservationType.BodyMassIndex, VitalMeasurementType.BodyMassIndex)]
        public void ConvertTest(ObservationType sourceObservationType, VitalMeasurementType targetVitalMeasurementType)
        {
            //Arrange

            //Act
            var result = Mapper.Map<VitalMeasurementType>(sourceObservationType);

            //Assert
            result.Should().Be(targetVitalMeasurementType);
        }

        [Test]
        public void ConvertTest_Throw_ServiceNotMappedException_ObservationType_BloodPressure()
        {
            //Arrange
            var source = ObservationType.BloodPressure;

            //Act
            Action action = () => Mapper.Map<VitalMeasurementType>(source);

            //Assert
            var result = action.Should().ThrowExactly<ServiceNotMappedException>();
            result.Which.Message.Should().Be("Could not map ObservationType to VitalMeasurementType");
        }
    }
}
