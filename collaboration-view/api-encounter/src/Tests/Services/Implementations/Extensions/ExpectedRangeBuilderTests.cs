// <copyright file="ExpectedRangeBuilderTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Services.Constants;
using Duly.CollaborationView.Encounter.Api.Services.Implementations.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations.Extensions
{
    [TestFixture]
    public class ExpectedRangeBuilderTests
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter should not span multiple lines", Justification = "TestData Parameters")]
        public static IEnumerable<TestCaseData> TestCasesWithResults
        {
            get
            {
                yield return new TestCaseData(
                    VitalsCardType.BloodPressure,
                    UnitsOfMeasureConstants.PressureUnitOfMeasure,
                    VitalsConstants.SystolicBloodPressureLabel,
                    new KeyValuePair<string, ExpectedRange<decimal>>[]
                    {
                    new("Normal", new ExpectedRange<decimal>()
                    {
                        Min = 90M,
                        Max = 120M
                    })
                    });
                yield return new TestCaseData(
                    VitalsCardType.BloodPressure,
                    UnitsOfMeasureConstants.PressureUnitOfMeasure,
                    VitalsConstants.DiastolicBloodPressureLabel,
                    new KeyValuePair<string, ExpectedRange<decimal>>[]
                    {
                    new("Normal", new ExpectedRange<decimal>()
                    {
                        Min = 60M,
                        Max = 80M
                    })
                    });
                yield return new TestCaseData(
                    VitalsCardType.BloodOxygen,
                    UnitsOfMeasureConstants.PercentUnitOfMeasure,
                    string.Empty,
                    new KeyValuePair<string, ExpectedRange<decimal>>[]
                    {
                    new("Normal", new ExpectedRange<decimal>()
                    {
                        Min = 95M,
                        Max = 100M
                    }),
                    new("Concerning", new ExpectedRange<decimal>()
                    {
                        InclusiveLowerBound = true,
                        InclusiveUpperBound = false,
                        Min = 91M,
                        Max = 95M
                    }),
                    new("Low", new ExpectedRange<decimal>()
                    {
                        InclusiveLowerBound = true,
                        InclusiveUpperBound = false,
                        Min = 86M,
                        Max = 91M
                    }),
                    new("Brain-Affecting", new ExpectedRange<decimal>()
                    {
                        InclusiveLowerBound = true,
                        InclusiveUpperBound = false,
                        Min = 80M,
                        Max = 86M
                    })
                    });
                yield return new TestCaseData(
                    VitalsCardType.RespiratoryRate,
                    UnitsOfMeasureConstants.RepetitionsPerMinuteUnitOfMeasure,
                    string.Empty,
                    new KeyValuePair<string, ExpectedRange<decimal>>[]
                    {
                        new("Normal", new ExpectedRange<decimal>()
                        {
                            Min = 12M,
                            Max = 20M
                        })
                    });
                yield return new TestCaseData(
                    VitalsCardType.BodyMassIndex,
                    null,
                    string.Empty,
                    new KeyValuePair<string, ExpectedRange<decimal>>[]
                    {
                        new("Normal", new ExpectedRange<decimal>()
                        {
                            Min = 18.5M,
                            Max = 24.9M
                        })
                    });
                yield return new TestCaseData(
                    VitalsCardType.BodyMassIndex,
                    string.Empty,
                    string.Empty,
                    new KeyValuePair<string, ExpectedRange<decimal>>[]
                    {
                        new("Normal", new ExpectedRange<decimal>()
                        {
                            Min = 18.5M,
                            Max = 24.9M
                        })
                    });
                yield return new TestCaseData(
                    VitalsCardType.HeartRate,
                    UnitsOfMeasureConstants.RepetitionsPerMinuteUnitOfMeasure,
                    string.Empty,
                    new KeyValuePair<string, ExpectedRange<decimal>>[]
                    {
                        new("Normal", new ExpectedRange<decimal>()
                        {
                            Min = 60M,
                            Max = 100M
                        })
                    });
                yield return new TestCaseData(
                    VitalsCardType.Temperature,
                    UnitsOfMeasureConstants.CelsiusTemperatureUnitOfMeasure,
                    string.Empty,
                    new KeyValuePair<string, ExpectedRange<decimal>>[]
                    {
                        new("Normal", new ExpectedRange<decimal>()
                        {
                            Min = 35.5M,
                            Max = 36.6M
                        })
                    });
                yield return new TestCaseData(
                    VitalsCardType.Temperature,
                    UnitsOfMeasureConstants.FahrenheitTemperatureUnitOfMeasure,
                    string.Empty,
                    new KeyValuePair<string, ExpectedRange<decimal>>[]
                    {
                        new("Normal", new ExpectedRange<decimal>()
                        {
                            Min = 96M,
                            Max = 99.5M
                        })
                    });
            }
        }

        [TestCaseSource(nameof(TestCasesWithResults))]
        public void BuildExpectedRangesTest_ShouldNotThrow_AndReturnResult(VitalsCardType vitalsCardType, string unitOfMeasure, string label, KeyValuePair<string, ExpectedRange<decimal>>[] expected)
        {
            KeyValuePair<string, ExpectedRange<decimal>>[] result = null;
            vitalsCardType.Invoking(x => result = x.BuildExpectedRanges(unitOfMeasure, label)).Should().NotThrow();
            result.Should().BeEquivalentTo(expected);
            result.Should().OnlyContain(x => x.Value.Max > x.Value.Min);
        }

        [TestCase(VitalsCardType.BloodPressure, "NewLabel", VitalsConstants.SystolicBloodPressureLabel)]
        [TestCase(VitalsCardType.BloodPressure, UnitsOfMeasureConstants.PressureUnitOfMeasure, "NewLabel")]
        [TestCase(VitalsCardType.BloodOxygen, "NewLabel", "")]
        [TestCase(VitalsCardType.BodyMassIndex, "NewLabel", "")]
        [TestCase(VitalsCardType.HeartRate, "NewLabel", "")]
        [TestCase(VitalsCardType.RespiratoryRate, "NewLabel", "")]
        [TestCase(VitalsCardType.Temperature, "NewLabel", "")]
        [TestCase(VitalsCardType.Temperature, "NewLabel", "")]
        [TestCase((VitalsCardType)69, "NewLabel", "")]
        public void BuildExpectedRangesTest_ShouldNotThrow_AndReturnNull(VitalsCardType vitalsCardType, string unitOfMeasure, string label)
        {
            KeyValuePair<string, ExpectedRange<decimal>>[] result =
                Array.Empty<KeyValuePair<string, ExpectedRange<decimal>>>();
            vitalsCardType.Invoking(x => result = x.BuildExpectedRanges(unitOfMeasure, label)).Should().NotThrow();
            result.Should().BeNull();
        }
    }
}
