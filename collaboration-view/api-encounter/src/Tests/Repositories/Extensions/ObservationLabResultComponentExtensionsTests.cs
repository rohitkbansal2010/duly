// <copyright file="ObservationLabResultComponentExtensionsTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Extensions;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Duly.CollaborationView.Encounter.Api.Tests.Repositories.Extensions
{
    [TestFixture]
    public class ObservationLabResultComponentExtensionsTests
    {
        [Test]
        public void CheckIfComponentIsAbnormalTest_ReturnTrue_ByInterpretation()
        {
            //Arrange
            var labResult = new ObservationLabResultComponent
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
                }
            };

            //Act
            var result = labResult.CheckIfComponentIsAbnormal();

            //Assert
            result.Should().BeTrue();
        }

        [Test]
        public void CheckIfComponentIsAbnormalTest_ReturnTrue_ByMeasurementRange()
        {
            //Arrange
            var labResult = new ObservationLabResultComponent
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
            };

            //Act
            var result = labResult.CheckIfComponentIsAbnormal();

            //Assert
            result.Should().BeTrue();
        }

        [Test]
        public void CheckIfComponentIsAbnormalTest_ReturnFalse()
        {
            //Arrange
            var labResult = new ObservationLabResultComponent
            {
                Interpretations = new[]
                {
                    new ObservationLabResultInterpretation
                    {
                        Code = "I",
                        Text = "Intermediate"
                    }
                },
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
            };

            //Act
            var result = labResult.CheckIfComponentIsAbnormal();

            //Assert
            result.Should().BeFalse();
        }
    }
}
