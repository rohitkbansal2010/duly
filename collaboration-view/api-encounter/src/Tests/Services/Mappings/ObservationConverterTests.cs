// <copyright file="ObservationConverterTests.cs" company="Duly Health and Care">
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
    internal class ObservationConverterTests : MapperConfigurator<RepositoryModelsToProcessContractsProfile>
    {
        [TestCase(ObservationType.PainLevel, VitalsCardType.PainLevel, VitalType.PainLevel, VitalMeasurementType.PainLevel, 10)]
        [TestCase(ObservationType.BodyMassIndex, VitalsCardType.BodyMassIndex, VitalType.BodyMassIndex, VitalMeasurementType.BodyMassIndex, null)]
        public void Convert_Observation_To_VitalsCard_Test(
            ObservationType sourceObservationType,
            VitalsCardType targetCardType,
            VitalType targetVitalType,
            VitalMeasurementType targetVitalMeasurementType,
            decimal? targetMaxScaleValue)
        {
            //Arrange
            var observation = new Observation
            {
                Components = new ObservationComponent[]
                {
                    new()
                    {
                        Measurements = new ObservationMeasurement[]
                        {
                            new()
                        }
                    }
                },
                Type = sourceObservationType
            };

            //Act
            var result = Mapper.Map<VitalsCard>(observation);

            //Assert
            result.Should().NotBeNull();
            result.CardType.Should().Be(targetCardType);
            result.Vitals.Should().NotBeEmpty();
            result.Vitals.Length.Should().Be(1);
            result.Vitals[0].VitalType.Should().Be(targetVitalType);
            result.Vitals[0].Measurements.Should().NotBeEmpty();
            result.Vitals[0].Measurements.Length.Should().Be(1);

            result.Vitals[0].Measurements[0].MeasurementType.Should().Be(targetVitalMeasurementType);
            result.Vitals[0].Measurements[0].MaxScaleValue.Should().Be(targetMaxScaleValue);
            result.Vitals[0].Measurements[0].Unit.Should().BeNull();
            result.Vitals[0].Measurements[0].Value.Should().Be(default);
            result.Vitals[0].Measurements[0].Measured.Should().Be(default);
        }

        [Test]
        public void Convert_Observation_BloodPressure_Test()
        {
            //Arrange
            var observation = new Observation
            {
                Components = new ObservationComponent[]
                {
                    new()
                    {
                        Measurements = new ObservationMeasurement[]
                        {
                            new()
                            {
                                Value = decimal.MinusOne,
                                Unit = "test"
                            }
                        },
                        Type = ObservationComponentType.Diastolic
                    }
                },
                Type = ObservationType.BloodPressure,
                Date = DateTimeOffset.UnixEpoch
            };

            //Act
            var result = Mapper.Map<VitalsCard>(observation);

            //Assert
            result.Vitals[0].Measurements[0].MeasurementType.Should().Be(VitalMeasurementType.DiastolicBloodPressure);
            result.Vitals[0].Measurements[0].MaxScaleValue.Should().BeNull();
            result.Vitals[0].Measurements[0].Unit.Should().Be("test");
            result.Vitals[0].Measurements[0].Value.Should().Be(decimal.MinusOne);
            result.Vitals[0].Measurements[0].Measured.Should().Be(DateTimeOffset.UnixEpoch);
        }

        [Test]
        public void Convert_Observation_BodyMassIndex_Test()
        {
            //Arrange
            var observation = new Observation
            {
                Components = new ObservationComponent[]
                {
                    new()
                    {
                        Measurements = new ObservationMeasurement[]
                        {
                            new()
                            {
                                Value = decimal.MinusOne,
                                Unit = "testW"
                            }
                        },
                        Type = ObservationComponentType.Weight
                    },
                    new()
                    {
                        Measurements = new ObservationMeasurement[]
                        {
                            new()
                            {
                                Value = decimal.One,
                                Unit = "testH"
                            }
                        },
                        Type = ObservationComponentType.Height
                    },
                    new()
                    {
                        Measurements = new ObservationMeasurement[]
                        {
                            new()
                            {
                                Value = decimal.Zero,
                                Unit = "testB"
                            }
                        },
                    }
                },
                Type = ObservationType.BodyMassIndex,
                Date = DateTimeOffset.UnixEpoch
            };

            //Act
            var result = Mapper.Map<VitalsCard>(observation);

            //Assert
            result.Vitals.Length.Should().Be(3);

            result.Vitals[0].Measurements[0].MeasurementType.Should().Be(VitalMeasurementType.BodyWeight);
            result.Vitals[0].Measurements[0].MaxScaleValue.Should().BeNull();
            result.Vitals[0].Measurements[0].Unit.Should().Be("testW");
            result.Vitals[0].Measurements[0].Value.Should().Be(decimal.MinusOne);
            result.Vitals[0].Measurements[0].Measured.Should().Be(DateTimeOffset.UnixEpoch);

            result.Vitals[1].Measurements[0].MeasurementType.Should().Be(VitalMeasurementType.BodyHeight);
            result.Vitals[1].Measurements[0].MaxScaleValue.Should().BeNull();
            result.Vitals[1].Measurements[0].Unit.Should().Be("testH");
            result.Vitals[1].Measurements[0].Value.Should().Be(decimal.One);
            result.Vitals[1].Measurements[0].Measured.Should().Be(DateTimeOffset.UnixEpoch);

            result.Vitals[2].Measurements[0].MeasurementType.Should().Be(VitalMeasurementType.BodyMassIndex);
            result.Vitals[2].Measurements[0].MaxScaleValue.Should().BeNull();
            result.Vitals[2].Measurements[0].Unit.Should().Be("testB");
            result.Vitals[2].Measurements[0].Value.Should().Be(decimal.Zero);
            result.Vitals[2].Measurements[0].Measured.Should().Be(DateTimeOffset.UnixEpoch);
        }
    }
}
