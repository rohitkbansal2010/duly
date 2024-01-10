// <copyright file="TestReportResultConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Mappings;
using Duly.CollaborationView.Encounter.Api.Tests.Common;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using ApiContracts = Duly.CollaborationView.Encounter.Api.Contracts;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Mappings
{
    [TestFixture]
    internal class TestReportResultConverterTests : MapperConfigurator<RepositoryModelsToProcessContractsProfile>
    {
        [Test]
        public void ConvertTest_With_ValueText()
        {
            ObservationLabResult source = new()
            {
                Id = Guid.NewGuid().ToString(),
                ComponentName = "UA Nitrite",
                Date = new(2000, 5, 5, 13, 45, 0, default),
                Components = new ObservationLabResultComponent[]
                {
                    new()
                    {
                        Interpretations = new[]
                        {
                            new ObservationLabResultInterpretation
                            {
                                Code = "W",
                                Text = "Worse"
                            },
                            new ObservationLabResultInterpretation
                            {
                                Code = "I",
                                Text = "Intermediate"
                            },
                            new ObservationLabResultInterpretation
                            {
                                Code = "A",
                                Text = "Abnormal"
                            }
                        },
                        ReferenceRange = new ObservationLabResultReferenceRange[]
                        {
                            new()
                            {
                                Text = "Negative"
                            },
                            new()
                            {
                                Text = "Sample"
                            },
                        },
                        ValueText = "Positive",
                        Notes = new[]
                        {
                            "A mutation in KRAS codon 12 was detected: c.35G>T, p.G12V"
                        }
                    }
                }
            };

            var expectedComponent = source.Components.First();
            var expectedReferenceRange = expectedComponent.ReferenceRange?.FirstOrDefault();
            var expectedMeasurement = expectedComponent.Measurements?.FirstOrDefault();

            //Act
            var result = Mapper.Map<ApiContracts.TestReportResult>(source);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(source.Id);

            result.IsAbnormalResult.Should().Be(true);

            result.Notes.Should().NotBeNullOrEmpty();
            result.Notes.Should().HaveCount(expectedComponent.Notes.Length);
            result.Notes[0].Should().Be(expectedComponent.Notes[0]);

            result.ReferenceRange.Should().NotBeNull();
            result.ReferenceRange.High.Should().BeNull();
            result.ReferenceRange.Low.Should().BeNull();
            result.ReferenceRange.Text.Should().Be(expectedReferenceRange.Text);

            result.Measurement.Should().BeNull();

            result.ValueText.Should().Be(expectedComponent.ValueText);

            result.Interpretations.Should().NotBeNullOrEmpty();
            result.Interpretations.Should().HaveCount(3);
            result.Interpretations.First().Should().NotBeNull();
            result.Interpretations.First().Should().Be("Worse");
        }

        [Test]
        public void ConvertTest_With_Measurement_Normal()
        {
            ObservationLabResult source = new()
            {
                Id = Guid.NewGuid().ToString(),
                ComponentName = "Sodium",
                Date = new(2000, 5, 5, 13, 45, 0, default),
                Components = new ObservationLabResultComponent[]
                {
                    new()
                    {
                        ReferenceRange = new ObservationLabResultReferenceRange[]
                        {
                            new()
                            {
                                Low = new ObservationLabResultMeasurement
                                {
                                    Value = 136M,
                                    Unit = "mmol/L"
                                },
                                High = new ObservationLabResultMeasurement
                                {
                                    Value = 145M,
                                    Unit = "mmol/L"
                                },
                                Text = "136 - 145 mmol/L"
                            }
                        },
                        Measurements = new ObservationLabResultMeasurement[]
                        {
                            new()
                            {
                                Value = 138M,
                                Unit = "mmol/L"
                            }
                        }
                    }
                }
            };

            var expectedComponent = source.Components.First();
            var expectedReferenceRange = expectedComponent.ReferenceRange?.FirstOrDefault();
            var expectedMeasurement = expectedComponent.Measurements?.FirstOrDefault();

            //Act
            var result = Mapper.Map<ApiContracts.TestReportResult>(source);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(source.Id);

            result.IsAbnormalResult.Should().Be(false);

            result.Notes.Should().BeNull();

            result.ReferenceRange.Should().NotBeNull();
            result.ReferenceRange.High.Should().NotBeNull();
            result.ReferenceRange.High.Value.Should().Be(expectedReferenceRange.High.Value);
            result.ReferenceRange.High.Unit.Should().Be(expectedReferenceRange.High.Unit);
            result.ReferenceRange.Low.Should().NotBeNull();
            result.ReferenceRange.Low.Value.Should().Be(expectedReferenceRange.Low.Value);
            result.ReferenceRange.Low.Unit.Should().Be(expectedReferenceRange.Low.Unit);
            result.ReferenceRange.Text.Should().Be(expectedReferenceRange.Text);

            result.ValueText.Should().BeNull();

            result.Measurement.Should().NotBeNull();
            result.Measurement.Value.Should().Be(expectedMeasurement.Value);
            result.Measurement.Unit.Should().Be(expectedMeasurement.Unit);
        }

        [Test]
        public void ConvertTest_With_Measurement_Abnormal()
        {
            ObservationLabResult source = new()
            {
                Id = Guid.NewGuid().ToString(),
                ComponentName = "Sodium",
                Date = new(2000, 5, 5, 13, 45, 0, default),
                Components = new ObservationLabResultComponent[]
                {
                    new()
                    {
                        ReferenceRange = new ObservationLabResultReferenceRange[]
                        {
                            new()
                            {
                                Low = new ObservationLabResultMeasurement
                                {
                                    Value = 136M,
                                    Unit = "mmol/L"
                                },
                                High = new ObservationLabResultMeasurement
                                {
                                    Value = 145M,
                                    Unit = "mmol/L"
                                },
                                Text = "136 - 145 mmol/L"
                            }
                        },
                        Measurements = new ObservationLabResultMeasurement[]
                        {
                            new()
                            {
                                Value = 150M,
                                Unit = "mmol/L"
                            }
                        }
                    }
                }
            };

            var expectedComponent = source.Components.First();
            var expectedReferenceRange = expectedComponent.ReferenceRange?.FirstOrDefault();
            var expectedMeasurement = expectedComponent.Measurements?.FirstOrDefault();

            //Act
            var result = Mapper.Map<ApiContracts.TestReportResult>(source);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(source.Id);

            result.IsAbnormalResult.Should().Be(true);

            result.Notes.Should().BeNull();

            result.ReferenceRange.Should().NotBeNull();
            result.ReferenceRange.High.Should().NotBeNull();
            result.ReferenceRange.High.Value.Should().Be(expectedReferenceRange.High.Value);
            result.ReferenceRange.High.Unit.Should().Be(expectedReferenceRange.High.Unit);
            result.ReferenceRange.Low.Should().NotBeNull();
            result.ReferenceRange.Low.Value.Should().Be(expectedReferenceRange.Low.Value);
            result.ReferenceRange.Low.Unit.Should().Be(expectedReferenceRange.Low.Unit);
            result.ReferenceRange.Text.Should().Be(expectedReferenceRange.Text);

            result.ValueText.Should().BeNull();

            result.Measurement.Should().NotBeNull();
            result.Measurement.Value.Should().Be(expectedMeasurement.Value);
            result.Measurement.Unit.Should().Be(expectedMeasurement.Unit);
        }
    }
}
