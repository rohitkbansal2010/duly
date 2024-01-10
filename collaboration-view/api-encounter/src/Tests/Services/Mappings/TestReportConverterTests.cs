// <copyright file="TestReportConverterTests.cs" company="Duly Health and Care">
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
    internal class TestReportConverterTests : MapperConfigurator<RepositoryModelsToProcessContractsProfile>
    {
        [Test]
        public void Convert_Without_HasAbnormalResults_Test()
        {
            //Arrange
            DiagnosticReport source = new()
            {
                Id = Guid.NewGuid().ToString(),
                EffectiveDate = new(2021, 1, 16, 0, 0, 0, default),
                Name = "Urinalysis",
                Status = DiagnosticReportStatus.Registered,
                Observations = new ObservationLabResult[]
                {
                    new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ComponentName = "UA Appearance",
                        Date = new(2000, 5, 5, 0, 0, 0, default),
                        Components = new ObservationLabResultComponent[]
                        {
                            new()
                            {
                                Interpretations = new[]
                                {
                                    new ObservationLabResultInterpretation
                                    {
                                        Code = "N",
                                        Text = "Normal"
                                    }
                                },
                                Measurements = Array.Empty<ObservationLabResultMeasurement>(),
                                ReferenceRange = new ObservationLabResultReferenceRange[]
                                {
                                    new() { Text = "<= Clear" },
                                },
                                ValueText = "Cloudy"
                            }
                        }
                    }
                }
            };

            //Act
            var result = Mapper.Map<TestReport>(source);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(source.Id);
            result.Title.Should().Be("Urinalysis");
            result.Date.Should().Be(source.EffectiveDate.Value);
            result.HasAbnormalResults.Should().BeFalse();
        }

        [Test]
        public void Convert_With_HasAbnormalResults_ByMeasurement_Test()
        {
            DiagnosticReport source = new()
            {
                Id = Guid.NewGuid().ToString(),
                EffectiveDate = new(2021, 8, 14, 9, 0, 0, default),
                Name = "Comprehensive metabolic panel",
                Status = DiagnosticReportStatus.Final,
                Issued = new(2021, 8, 12, 14, 45, 0, default),
                Observations = new ObservationLabResult[]
                {
                    new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ComponentName = "Sodium, Serum",
                        Date = new(2000, 5, 5, 0, 0, 0, default),
                        Components = new ObservationLabResultComponent[]
                        {
                            new()
                            {
                                Interpretations = Array.Empty<ObservationLabResultInterpretation>(),
                                Measurements = new ObservationLabResultMeasurement[]
                                {
                                    new() { Value = 144, Unit = "mg/dL" },
                                },
                                ReferenceRange = new ObservationLabResultReferenceRange[]
                                {
                                    new()
                                    {
                                        Low = new() { Value = 134, Unit = "mg/dL" },
                                        High = new() { Value = 144, Unit = "mg/dL" },
                                    },
                                },
                            }
                        }
                    },
                    new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ComponentName = "Total cholesterol",
                        Date = new(1917, 11, 7, 0, 0, 0, default),
                        Components = new ObservationLabResultComponent[]
                        {
                            new()
                            {
                                Interpretations = new[]
                                {
                                    new ObservationLabResultInterpretation
                                    {
                                        Code = "N",
                                        Text = "Normal"
                                    }
                                },
                                Measurements = Array.Empty<ObservationLabResultMeasurement>(),
                                ReferenceRange = new ObservationLabResultReferenceRange[]
                                {
                                    new() { Text = "Negative" },
                                },
                                ValueText = "Negative"
                            },
                            new()
                            {
                                Interpretations = new[]
                                {
                                    new ObservationLabResultInterpretation
                                    {
                                        Code = "N",
                                        Text = "Normal"
                                    },
                                    new ObservationLabResultInterpretation
                                    {
                                        Code = "I",
                                        Text = "Intermediate"
                                    }
                                },
                                Measurements = new ObservationLabResultMeasurement[]
                                {
                                    new() { Value = 324, Unit = "mg/dL" },
                                    new() { Value = 35, Unit = "Celsius" },
                                    new() { Value = 224, Unit = "mg/dL" },
                                    new() { Value = 124, Unit = "mg/dL" },
                                },
                                ReferenceRange = new ObservationLabResultReferenceRange[]
                                {
                                    new()
                                    {
                                        Low = new() { Value = 100, Unit = "mg/dL" },
                                        High = new() { Value = 200, Unit = "mg/dL" },
                                    },
                                    new()
                                    {
                                        Low = new() { Value = 300, Unit = "mg/dL" },
                                        High = new() { Value = 400, Unit = "mg/dL" },
                                    },
                                },
                            },
                        }
                    }
                }
            };

            //Act
            var result = Mapper.Map<TestReport>(source);

            //Assert
            result.Should().NotBeNull();
            result.HasAbnormalResults.Should().BeTrue();
        }

        [Test]
        public void Convert_With_HasAbnormalResults_ByInterpretation_Test()
        {
            DiagnosticReport source = new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Lorem Ipsum",
                EffectiveDate = new(2020, 2, 8, 0, 0, 0, default),
                Status = DiagnosticReportStatus.Amended,
                Observations = new ObservationLabResult[]
                {
                    new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ComponentName = "UA Nitrite",
                        Date = new(2000, 5, 5, 0, 0, 0, default),
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
                                        Code = "N",
                                        Text = "Normal"
                                    },
                                    new ObservationLabResultInterpretation
                                    {
                                        Code = "A",
                                        Text = "Abnormal"
                                    }
                                },
                                ReferenceRange = new ObservationLabResultReferenceRange[]
                                {
                                    new() { Text = "Negative" },
                                    new() { Text = "Sample" },
                                },
                                ValueText = "Positive"
                            }
                        }
                    }
                }
            };

            //Act
            var result = Mapper.Map<TestReport>(source);

            //Assert
            result.Should().NotBeNull();
            result.HasAbnormalResults.Should().BeTrue();
        }
    }
}
