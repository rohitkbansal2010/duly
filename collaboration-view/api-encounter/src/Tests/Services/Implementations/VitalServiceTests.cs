// <copyright file="VitalServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Implementations;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations
{
    [TestFixture]
    public class VitalServiceTests
    {
        /// <summary>
        /// The last three years.
        /// </summary>
        private const int YearsToAddToTheStartDate = 3;
        private const int DaysToAddToTheStartDate = 1;

        private Mock<IObservationRepository> _repositoryMocked;
        private Mock<IMapper> _mapperMocked;

        [SetUp]
        public void SetUp()
        {
            _repositoryMocked = new Mock<IObservationRepository>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]
        public async Task GetLatestVitalsForPatientAsync_BuildMissedVitalsCard_Test()
        {
            //Arrange
            const string patientId = "test";
            IVitalService serviceMocked = new VitalService(_repositoryMocked.Object, _mapperMocked.Object);

            //Act
            var result = await serviceMocked.GetLatestVitalsForPatientAsync(patientId);

            //Assert
            var vitalsCard = result.ToArray();
            vitalsCard.Should().NotBeEmpty();
            vitalsCard.Length.Should().Be(8);

            vitalsCard.All(card => card.Vitals.Length == 0).Should().BeTrue();

            var cardTypes = (VitalsCardType[])Enum.GetValues(typeof(VitalsCardType));
            for (var i = 0; i < cardTypes.Length; i++)
            {
                vitalsCard[i].CardType.Should().Be(cardTypes[i]);
            }
        }

        [Test]
        public async Task GetLatestVitalsForPatientAsync_WeightAndHeight_Test()
        {
            //Arrange
            const string patientId = "test";
            var observationTypes = (ObservationType[])Enum.GetValues(typeof(ObservationType));
            IEnumerable<Observation> observations = new Observation[] { new() };
            var vitalsCards = BuildWeightAndHeightVitalsCards();

            _repositoryMocked
                .Setup(repository => repository.GetLatestObservationsForPatientAsync(patientId, observationTypes))
                .Returns(Task.FromResult(observations));

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<VitalsCard>>(observations))
                .Returns(vitalsCards);

            IVitalService serviceMocked = new VitalService(_repositoryMocked.Object, _mapperMocked.Object);

            //Act
            var result = await serviceMocked.GetLatestVitalsForPatientAsync(patientId);

            //Assert
            result.Should().NotBeEmpty();
            var weightAndHeight = result.First(card => card.CardType == VitalsCardType.WeightAndHeight);
            weightAndHeight.Vitals.Length.Should().Be(2);
            weightAndHeight.Vitals[0].VitalType.Should().Be(VitalType.Weight);
            weightAndHeight.Vitals[0].Measurements.Length.Should().Be(1);
            weightAndHeight.Vitals[0].Measurements[0].Unit.Should().Be("lbs");
            weightAndHeight.Vitals[1].VitalType.Should().Be(VitalType.Height);
            weightAndHeight.Vitals[1].Measurements.Length.Should().Be(1);
            weightAndHeight.Vitals[1].Measurements[0].Unit.Should().Be("ft");
        }

        [Test]
        public async Task GetLatestVitalsForPatientAsync_Temperature_Test()
        {
            //Arrange
            const string patientId = "test";
            var observationTypes = (ObservationType[])Enum.GetValues(typeof(ObservationType));
            IEnumerable<Observation> observations = new Observation[] { new() };
            var vitalsCards = BuildTemperatureVitalsCards();

            _repositoryMocked
                .Setup(repository => repository.GetLatestObservationsForPatientAsync(patientId, observationTypes))
                .Returns(Task.FromResult(observations));

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<VitalsCard>>(observations))
                .Returns(vitalsCards);

            IVitalService serviceMocked = new VitalService(_repositoryMocked.Object, _mapperMocked.Object);

            //Act
            var result = await serviceMocked.GetLatestVitalsForPatientAsync(patientId);

            //Assert
            result.Should().NotBeEmpty();
            var temperature = result.First(card => card.CardType == VitalsCardType.Temperature);
            temperature.Vitals.Length.Should().Be(1);
            temperature.Vitals[0].VitalType.Should().Be(VitalType.Temperature);
            temperature.Vitals[0].Measurements.Length.Should().Be(1);
            temperature.Vitals[0].Measurements[0].Unit.Should().Be("[degF]");
        }

        [Test]
        public async Task GetLatestVitalsForPatientAsync_Temperature_WithoutRequiredUnit_Test()
        {
            //Arrange
            const string patientId = "test";
            var observationTypes = (ObservationType[])Enum.GetValues(typeof(ObservationType));
            IEnumerable<Observation> observations = new Observation[] { new() };
            var vitalsCards = BuildTemperatureVitalsCards(false);

            _repositoryMocked
                .Setup(repository => repository.GetLatestObservationsForPatientAsync(patientId, observationTypes))
                .Returns(Task.FromResult(observations));

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<VitalsCard>>(observations))
                .Returns(vitalsCards);

            IVitalService serviceMocked = new VitalService(_repositoryMocked.Object, _mapperMocked.Object);

            //Act
            var result = await serviceMocked.GetLatestVitalsForPatientAsync(patientId);

            //Assert
            result.Should().NotBeEmpty();
            var temperature = result.First(card => card.CardType == VitalsCardType.Temperature);
            temperature.Vitals.Length.Should().Be(1);
            temperature.Vitals[0].VitalType.Should().Be(VitalType.Temperature);
            temperature.Vitals[0].Measurements.Length.Should().Be(1);
            temperature.Vitals[0].Measurements[0].Unit.Should().Be("c");
        }

        [Test]
        public async Task GetVitalHistoryForPatientByVitalsCardType_Empty_Test()
        {
            //Arrange
            const string patientId = "test";
            var startDate = DateTime.Now;
            const VitalsCardType vitalsCardType = VitalsCardType.PainLevel;

            IVitalService serviceMocked = new VitalService(_repositoryMocked.Object, _mapperMocked.Object);

            //Act
            var result = await serviceMocked.GetVitalHistoryForPatientByVitalsCardType(patientId, startDate, vitalsCardType);

            //Assert
            result.Should().BeNull();
            _repositoryMocked.Verify(repository => repository.GetObservationsForPatientAsync(patientId, BuildLowerBound(startDate), BuildUpperBound(startDate), It.IsAny<ObservationType[]>()));
        }

        [Test]
        public async Task GetVitalHistoryForPatientByVitalsCardType_Test()
        {
            //Arrange
            const string patientId = "test";
            var startDate = DateTime.Now;
            const VitalsCardType vitalsCardType = VitalsCardType.PainLevel;

            var observationTypes = new[] { ObservationType.PainLevel };

            _mapperMocked
                .Setup(mapper => mapper.Map<ObservationType[]>(vitalsCardType))
                .Returns(observationTypes);
            IEnumerable<Observation> observations = new[]
            {
                new Observation
                {
                    Components = new ObservationComponent[]
                    {
                        new()
                        {
                            Measurements = new[]
                            {
                                new ObservationMeasurement()
                            }
                        }
                    },
                    Type = ObservationType.PainLevel
                }
            };

            _repositoryMocked
                .Setup(repository =>
                    repository.GetObservationsForPatientAsync(patientId, BuildLowerBound(startDate), BuildUpperBound(startDate), observationTypes))
                .Returns(Task.FromResult(observations));

            IVitalService serviceMocked = new VitalService(_repositoryMocked.Object, _mapperMocked.Object);

            //Act
            var result = await serviceMocked.GetVitalHistoryForPatientByVitalsCardType(patientId, startDate, vitalsCardType);

            //Assert
            result.Should().NotBeNull();
            result.Chart.Should().NotBeNull();
            result.Chart.ChartOptions.Should().NotBeNull();
            result.Chart.ChartOptions.ChartScales.Should().NotBeNull();
            result.Chart.ChartOptions.ChartScales.YAxis.Should().NotBeNull();
            result.Chart.ChartOptions.ChartScales.YAxis.Min.Should().Be(1);
            result.Chart.ChartOptions.ChartScales.YAxis.Max.Should().Be(10);
            result.Chart.ChartOptions.ChartScales.YAxis.StepSize.Should().Be(2);
            result.Chart.ChartOptions.ChartScales.XAxis.Should().BeNull();

            result.Chart.Datasets.Should().NotBeEmpty();
            result.Chart.Datasets.Length.Should().Be(1);
            result.Chart.Datasets[0].Should().NotBeNull();
            result.Chart.Datasets[0].Label.Should().Be("Pain Level");
            result.Chart.Datasets[0].Ranges.Should().BeNull();
            result.Chart.Datasets[0].Visible.Should().BeTrue();

            result.Chart.Datasets[0].Data.Should().NotBeNull();
            result.Chart.Datasets[0].Data.Dimension.Should().Be(null);
            result.Chart.Datasets[0].Data.Values.Should().NotBeEmpty();
            result.Chart.Datasets[0].Data.Values.Length.Should().Be(1);
        }

        private static IEnumerable<VitalsCard> BuildWeightAndHeightVitalsCards()
        {
            IEnumerable<VitalsCard> vitalsCards = new[]
            {
                new VitalsCard
                {
                    CardType = VitalsCardType.WeightAndHeight,
                    Vitals = new Vital[]
                    {
                        new()
                        {
                            Measurements = new VitalMeasurement[]
                            {
                                new()
                                {
                                    Value = 12,
                                    Unit = "lbs"
                                },
                                new()
                                {
                                    Value = 12,
                                    Unit = "kg"
                                }
                            },
                            VitalType = VitalType.Weight
                        },
                        new()
                        {
                            Measurements = new VitalMeasurement[]
                            {
                                new()
                                {
                                    Value = 12,
                                    Unit = "ft"
                                },
                                new()
                                {
                                    Value = 12,
                                    Unit = "m"
                                }
                            },
                            VitalType = VitalType.Height
                        }
                    }
                }
            };
            return vitalsCards;
        }

        private static IEnumerable<VitalsCard> BuildTemperatureVitalsCards(bool adddegF = true)
        {
            VitalMeasurement[] vitalMeasurements;
            if (adddegF)
            {
                vitalMeasurements = new VitalMeasurement[]
                {
                    new()
                    {
                        Value = 12,
                        Unit = "c"
                    },
                    new()
                    {
                        Value = 12,
                        Unit = "[degF]"
                    }
                };
            }
            else
            {
                vitalMeasurements = new VitalMeasurement[]
                {
                    new()
                    {
                        Value = 12,
                        Unit = "c"
                    },
                    new()
                    {
                        Value = 12,
                        Unit = "k"
                    }
                };
            }

            IEnumerable<VitalsCard> vitalsCards = new[]
            {
                new VitalsCard
                {
                    CardType = VitalsCardType.Temperature,
                    Vitals = new Vital[]
                    {
                        new()
                        {
                            Measurements = vitalMeasurements,
                            VitalType = VitalType.Temperature
                        }
                    }
                }
            };
            return vitalsCards;
        }

        private static DateTimeOffset BuildLowerBound(DateTime time)
        {
            var lowerBound = time.AddYears(-YearsToAddToTheStartDate);
            lowerBound = DateTime.SpecifyKind(lowerBound, DateTimeKind.Utc);
            return new DateTimeOffset(lowerBound, TimeSpan.Zero);
        }

        private static DateTimeOffset BuildUpperBound(DateTime time)
        {
            var upperBound = time.AddDays(DaysToAddToTheStartDate);
            upperBound = DateTime.SpecifyKind(upperBound, DateTimeKind.Utc);
            return new DateTimeOffset(upperBound, TimeSpan.Zero);
        }
    }
}
