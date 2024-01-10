// <copyright file="VitalHistoryBuilderTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Implementations.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations.Extensions
{
    [TestFixture]
    public class VitalHistoryBuilderTests
    {
        [Test]
        public void BuildVitalHistory_BuildChartOptions_Test()
        {
            //Arrange
            const VitalsCardType vitalsCardType = VitalsCardType.HeartRate;
            var observations = new Observation[]
            {
                new()
                {
                    Type = ObservationType.HeartRate,
                    Components = new ObservationComponent[]
                    {
                        new()
                        {
                            Measurements = new ObservationMeasurement[]
                            {
                                new()
                                {
                                    Unit = "/min",
                                    Value = 2
                                }
                            }
                        }
                    },
                    Date = DateTimeOffset.Now
                }
            };

            //Act
            var result = VitalHistoryBuilder.BuildVitalHistory(vitalsCardType, observations);

            //Assert
            result.Should().NotBeNull();

            result.Chart.Should().NotBeNull();
            result.Chart.ChartOptions.Should().NotBeNull();
            result.Chart.ChartOptions.ChartScales.Should().NotBeNull();
            result.Chart.ChartOptions.ChartScales.YAxis.Should().NotBeNull();
            result.Chart.ChartOptions.ChartScales.XAxis.Should().BeNull();
        }

        [Test]
        public void BuildVitalHistory_BuildDataset_WeightAndHeight_Test()
        {
            //Arrange
            const VitalsCardType vitalsCardType = VitalsCardType.WeightAndHeight;
            var observations = new Observation[]
            {
                new()
                {
                    Type = ObservationType.BodyWeight,
                    Components = new ObservationComponent[]
                    {
                        new()
                        {
                            Measurements = new ObservationMeasurement[]
                            {
                                new()
                                {
                                    Unit = "test1",
                                    Value = 2
                                }
                            }
                        }
                    },
                    Date = DateTimeOffset.Now
                }
            };

            //Act
            var result = VitalHistoryBuilder.BuildVitalHistory(vitalsCardType, observations);

            //Assert
            result.Should().NotBeNull();

            result.Chart.Should().NotBeNull();
            result.Chart.Datasets.Should().NotBeEmpty();
            result.Chart.Datasets.Length.Should().Be(1);
            result.Chart.Datasets[0].Should().NotBeNull();
            result.Chart.Datasets[0].Label.Should().Be("Weight");
            result.Chart.Datasets[0].Ranges.Should().BeNull();
            result.Chart.Datasets[0].Visible.Should().BeTrue();

            result.Chart.Datasets[0].Data.Dimension.Should().Be("test1");
            result.Chart.Datasets[0].Data.Values.Should().NotBeEmpty();
            result.Chart.Datasets[0].Data.Values.Length.Should().Be(1);
            result.Chart.Datasets[0].Data.Values[0].X.Should().Be(observations[0].Date);
            result.Chart.Datasets[0].Data.Values[0].Y.Should().Be(2);
        }

        [Test]
        public void BuildVitalHistory_BuildDataset_Temperature_Test()
        {
            //Arrange
            const VitalsCardType vitalsCardType = VitalsCardType.Temperature;
            var observations = new Observation[]
            {
                new()
                {
                    Type = ObservationType.BodyTemperature,
                    Components = new ObservationComponent[]
                    {
                        new()
                        {
                            Measurements = new ObservationMeasurement[]
                            {
                                new()
                                {
                                    Unit = "[degF]",
                                    Value = 1
                                }
                            }
                        },
                        new()
                        {
                            Measurements = new ObservationMeasurement[]
                            {
                                new()
                                {
                                    Unit = "[degF]",
                                    Value = 2
                                }
                            }
                        }
                    },

                    Date = DateTimeOffset.Now
                },
                new()
                {
                    Type = ObservationType.BodyTemperature,
                    Components = new ObservationComponent[]
                    {
                        new()
                        {
                            Measurements = new ObservationMeasurement[]
                            {
                                new()
                                {
                                    Unit = "[degF]",
                                    Value = 3
                                }
                            }
                        },
                        new()
                        {
                            Measurements = new ObservationMeasurement[]
                            {
                                new()
                                {
                                    Unit = "[degF]",
                                    Value = 4
                                }
                            }
                        }
                    },

                    Date = DateTimeOffset.Now
                }
            };

            //Act
            var result = VitalHistoryBuilder.BuildVitalHistory(vitalsCardType, observations);

            //Assert
            result.Should().NotBeNull();

            result.Chart.Should().NotBeNull();
            result.Chart.Datasets.Should().NotBeEmpty();
            result.Chart.Datasets.Length.Should().Be(1);
            result.Chart.Datasets[0].Should().NotBeNull();
            result.Chart.Datasets[0].Label.Should().Be("Temperature");
            result.Chart.Datasets[0].Ranges.Should().NotBeNull();
            result.Chart.Datasets[0].Visible.Should().BeTrue();

            result.Chart.Datasets[0].Data.Dimension.Should().Be("[degF]");
            result.Chart.Datasets[0].Data.Values.Should().NotBeEmpty();
            result.Chart.Datasets[0].Data.Values.Length.Should().Be(2);
            result.Chart.Datasets[0].Data.Values[0].X.Should().Be(observations[0].Date);
            result.Chart.Datasets[0].Data.Values[0].Y.Should().Be(1);
        }

        [Test]
        public void BuildVitalHistory_BuildDataset_BloodPressure_Test()
        {
            //Arrange
            const VitalsCardType vitalsCardType = VitalsCardType.BloodPressure;
            var observations = new Observation[]
            {
                new()
                {
                    Type = ObservationType.BloodPressure,
                    Components = new ObservationComponent[]
                    {
                        new()
                        {
                            Measurements = new ObservationMeasurement[]
                            {
                                new()
                                {
                                    Unit = "mm[Hg]",
                                    Value = 1
                                }
                            },
                            Type = ObservationComponentType.Diastolic
                        },
                        new()
                        {
                            Measurements = new ObservationMeasurement[]
                            {
                                new()
                                {
                                    Unit = "mm[Hg]",
                                    Value = 2
                                }
                            },
                            Type = ObservationComponentType.Diastolic
                        }
                    },

                    Date = DateTimeOffset.Now
                },
                new()
                {
                    Type = ObservationType.BloodPressure,
                    Components = new ObservationComponent[]
                    {
                        new()
                        {
                            Measurements = new ObservationMeasurement[]
                            {
                                new()
                                {
                                    Unit = "mm[Hg]",
                                    Value = 3
                                }
                            },
                            Type = ObservationComponentType.Systolic
                        },
                        new()
                        {
                            Measurements = new ObservationMeasurement[]
                            {
                                new()
                                {
                                    Unit = "mm[Hg]",
                                    Value = 4
                                }
                            },
                            Type = ObservationComponentType.Systolic
                        }
                    },

                    Date = DateTimeOffset.Now
                }
            };

            //Act
            var result = VitalHistoryBuilder.BuildVitalHistory(vitalsCardType, observations);

            //Assert
            result.Should().NotBeNull();

            result.Chart.Should().NotBeNull();
            result.Chart.Datasets.Should().NotBeEmpty();
            result.Chart.Datasets.Length.Should().Be(2);
            result.Chart.Datasets[0].Should().NotBeNull();
            result.Chart.Datasets[0].Label.Should().Be("Diastolic");
            result.Chart.Datasets[0].Ranges.Should().NotBeNull();
            result.Chart.Datasets[0].Visible.Should().BeTrue();

            result.Chart.Datasets[0].Data.Dimension.Should().Be("mm[Hg]");
            result.Chart.Datasets[0].Data.Values.Should().NotBeEmpty();
            result.Chart.Datasets[0].Data.Values.Length.Should().Be(2);
            result.Chart.Datasets[0].Data.Values[0].X.Should().Be(observations[0].Date);
            result.Chart.Datasets[0].Data.Values[0].Y.Should().Be(1);

            result.Chart.Datasets[1].Should().NotBeNull();
            result.Chart.Datasets[1].Label.Should().Be("Diastolic");
            result.Chart.Datasets[1].Ranges.Should().NotBeNull();
            result.Chart.Datasets[1].Visible.Should().BeTrue();

            result.Chart.Datasets[1].Data.Dimension.Should().Be("mm[Hg]");
            result.Chart.Datasets[1].Data.Values.Should().NotBeEmpty();
            result.Chart.Datasets[1].Data.Values.Length.Should().Be(2);
            result.Chart.Datasets[1].Data.Values[0].X.Should().Be(observations[0].Date);
            result.Chart.Datasets[1].Data.Values[0].Y.Should().Be(1);
        }
    }
}
