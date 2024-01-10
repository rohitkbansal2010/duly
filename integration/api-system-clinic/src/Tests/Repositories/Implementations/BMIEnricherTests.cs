// <copyright file="BMIEnricherTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Repositories.Implementations;
using Duly.Clinic.Contracts;
using FluentAssertions;
using NUnit.Framework;

namespace Duly.Clinic.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class BMIEnricherTests
    {
        [Test]
        public void EnrichBMI_Should_ReturnCorrectResult()
        {
            var observation = new Observation()
            {
                Type = ObservationType.BodyMassIndex,
                Components = new[]
                {
                    new ObservationComponent()
                    {
                        Type = null,
                        Measurements = new ObservationMeasurement[]
                        {
                            new ObservationMeasurement()
                            {
                                Value = 1,
                                Unit = "Test"
                            }
                        }
                    }
                }
            };
            var complimemntaryObservations = new[]
            {
                new Observation()
                {
                    Type = ObservationType.BodyHeight,
                    Components = new[]
                    {
                    new ObservationComponent()
                    {
                    Type = null,
                    Measurements = new ObservationMeasurement[]
                    {
                        new ObservationMeasurement()
                        {
                            Value = 2,
                            Unit = "TestHeight"
                        }
                    }
                    }
                    }
                },
                new Observation()
                {
                    Type = ObservationType.BodyWeight,
                    Components = new[]
                    {
                    new ObservationComponent()
                    {
                    Type = null,
                    Measurements = new ObservationMeasurement[]
                    {
                        new ObservationMeasurement()
                        {
                            Value = 3,
                            Unit = "TestWeight"
                        }
                    }
                    }
                    }
                }
            };

            new BMIEnricher().EnrichBMI( observation, complimemntaryObservations);

            observation.Components.Length.Should().Be(3);
            observation.Components[0].Type.Should().BeNull();
            observation.Components[0].Measurements[0].Unit.Should().Be("Test");
            observation.Components[0].Measurements[0].Value.Should().Be(1);
            observation.Components[1].Type.Should().Be(ObservationComponentType.Weight);
            observation.Components[1].Measurements[0].Unit.Should().Be("TestWeight");
            observation.Components[1].Measurements[0].Value.Should().Be(3);
            observation.Components[2].Type.Should().Be(ObservationComponentType.Height);
            observation.Components[2].Measurements[0].Unit.Should().Be("TestHeight");
            observation.Components[2].Measurements[0].Value.Should().Be(2);
        }

        [Test]
        public void EnrichBMI_Should_ReturnCNotModifyResult()
        {
            var observation = new Observation()
            {
                Type = ObservationType.BodyMassIndex,
                Components = new[]
                {
                    new ObservationComponent()
                    {
                        Type = null,
                        Measurements = new ObservationMeasurement[]
                        {
                            new ObservationMeasurement()
                            {
                                Value = 1,
                                Unit = "Test"
                            }
                        }
                    }
                }
            };
            var complimemntaryObservations = new[]
            {
                new Observation()
                {
                    Type = ObservationType.BodyHeight,
                    Components = new[]
                    {
                    new ObservationComponent()
                    {
                    Type = null,
                    Measurements = new ObservationMeasurement[]
                    {
                        new ObservationMeasurement()
                        {
                            Value = 2,
                            Unit = "TestHeight"
                        }
                    }
                    }
                    }
                },
                new Observation()
                {
                    Type = ObservationType.BodyWeight,
                    Components = new[]
                    {
                    new ObservationComponent()
                    {
                    Type = ObservationComponentType.Systolic,
                    Measurements = new ObservationMeasurement[]
                    {
                        new ObservationMeasurement()
                        {
                            Value = 3,
                            Unit = "TestWeight"
                        }
                    }
                    }
                    }
                }
            };

            new BMIEnricher().EnrichBMI(observation, complimemntaryObservations);

            observation.Components.Length.Should().Be(1);
            observation.Components[0].Type.Should().BeNull();
            observation.Components[0].Measurements[0].Unit.Should().Be("Test");
            observation.Components[0].Measurements[0].Value.Should().Be(1);
        }

        [Test]
        public void EnrichBMI_Should_NotAddOtherMeasurements()
        {
            var observation = new Observation()
            {
                Type = ObservationType.BodyMassIndex,
                Components = new[]
                {
                    new ObservationComponent()
                    {
                        Type = null,
                        Measurements = new ObservationMeasurement[]
                        {
                            new ObservationMeasurement()
                            {
                                Value = 1,
                                Unit = "Test"
                            }
                        }
                    }
                }
            };
            var complimemntaryObservations = new[]
            {
                new Observation()
                {
                    Type = ObservationType.BloodPressure,
                    Components = new[]
                    {
                    new ObservationComponent()
                    {
                    Type = null,
                    Measurements = new ObservationMeasurement[]
                    {
                        new ObservationMeasurement()
                        {
                            Value = 2,
                            Unit = "TestHeight"
                        }
                    }
                    }
                    }
                },
                new Observation()
                {
                    Type = ObservationType.HeartRate,
                    Components = new[]
                    {
                    new ObservationComponent()
                    {
                    Type = null,
                    Measurements = new ObservationMeasurement[]
                    {
                        new ObservationMeasurement()
                        {
                            Value = 3,
                            Unit = "TestWeight"
                        }
                    }
                    }
                    }
                }
            };

            new BMIEnricher().EnrichBMI( observation, complimemntaryObservations);

            observation.Components.Length.Should().Be(1);
        }
    }
}