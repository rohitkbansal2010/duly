// <copyright file="ExpectedRangeBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Services.Constants;
using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations.Extensions
{
    internal static class ExpectedRangeBuilder
    {
        private const string Normal = "Normal";

        //Labels for Blood Oxygen expected ranges
        private const string Concerning = "Concerning";
        private const string Low = "Low";
        private const string BrainAffecting = "Brain-Affecting";

        /// <summary>
        /// Tries to build expected ranges for combination of unit and label.
        /// Returns null if not successful.
        /// </summary>
        /// <param name="vitalsCardType">Type of vital card.</param>
        /// <param name="unitOfMeasure">String representing units of measure.</param>
        /// <param name="label">Label with additional information.</param>
        /// <returns>Ranges if successful or null if not.</returns>
        public static KeyValuePair<string, ExpectedRange<decimal>>[] BuildExpectedRanges(this VitalsCardType vitalsCardType, string unitOfMeasure, string label)
        {
            return vitalsCardType switch
            {
                VitalsCardType.Temperature => GetTemperatureRangesByUnitOfMeasure(unitOfMeasure),
                VitalsCardType.BloodOxygen => GetBloodOxygenRangesByUnitOfMeasure(unitOfMeasure),
                VitalsCardType.BloodPressure => GetBloodPressureRangesByUnitOfMeasureAndLabel(unitOfMeasure, label),
                VitalsCardType.BodyMassIndex => GetBodyMassIndexRangesByUnitOfMeasure(unitOfMeasure),
                VitalsCardType.HeartRate => GetHeartRateRangesByUnitOfMeasure(unitOfMeasure),
                VitalsCardType.RespiratoryRate => GetRespiratoryRateRangesByUnitOfMeasure(unitOfMeasure),
                _ => null
            };
        }

        private static KeyValuePair<string, ExpectedRange<decimal>>[] GetTemperatureRangesByUnitOfMeasure(string unitOfMeasure)
        {
            return unitOfMeasure switch
            {
                UnitsOfMeasureConstants.CelsiusTemperatureUnitOfMeasure => CreateNormalRange(35.5M, 36.6M),
                UnitsOfMeasureConstants.FahrenheitTemperatureUnitOfMeasure => CreateNormalRange(96M, 99.5M),
                _ => null
            };
        }

        private static KeyValuePair<string, ExpectedRange<decimal>>[] GetHeartRateRangesByUnitOfMeasure(string unitOfMeasure)
        {
            return unitOfMeasure switch
            {
                UnitsOfMeasureConstants.RepetitionsPerMinuteUnitOfMeasure => CreateNormalRange(60M, 100M),
                _ => null
            };
        }

        private static KeyValuePair<string, ExpectedRange<decimal>>[] GetBloodPressureRangesByUnitOfMeasureAndLabel(string unitOfMeasure, string label)
        {
            return unitOfMeasure switch
            {
                UnitsOfMeasureConstants.PressureUnitOfMeasure => GetRangesForBloodPressureByLabel(label),
                _ => null
            };
        }

        private static KeyValuePair<string, ExpectedRange<decimal>>[] GetRangesForBloodPressureByLabel(string label)
        {
            return label switch
            {
                VitalsConstants.SystolicBloodPressureLabel => CreateNormalRange(90M, 120M),
                VitalsConstants.DiastolicBloodPressureLabel => CreateNormalRange(60M, 80M),
                _ => null
            };
        }

        private static KeyValuePair<string, ExpectedRange<decimal>>[] GetBloodOxygenRangesByUnitOfMeasure(string unitOfMeasure)
        {
            return unitOfMeasure switch
            {
                UnitsOfMeasureConstants.PercentUnitOfMeasure => new KeyValuePair<string, ExpectedRange<decimal>>[]
                {
                    new(Normal, new ExpectedRange<decimal>()
                    {
                        Min = 95M,
                        Max = 100M
                    }),
                    new(Concerning, new ExpectedRange<decimal>()
                    {
                        InclusiveLowerBound = true,
                        InclusiveUpperBound = false,
                        Min = 91M,
                        Max = 95M
                    }),
                    new(Low, new ExpectedRange<decimal>()
                    {
                        InclusiveLowerBound = true,
                        InclusiveUpperBound = false,
                        Min = 86M,
                        Max = 91M
                    }),
                    new(BrainAffecting, new ExpectedRange<decimal>()
                    {
                        InclusiveLowerBound = true,
                        InclusiveUpperBound = false,
                        Min = 80M,
                        Max = 86M,
                    })
                },
                _ => null
            };
        }

        private static KeyValuePair<string, ExpectedRange<decimal>>[] GetBodyMassIndexRangesByUnitOfMeasure(string unitOfMeasure)
        {
            return unitOfMeasure switch
            {
                var unit when string.IsNullOrEmpty(unit) => CreateNormalRange(18.5M, 24.9M),
                _ => null
            };
        }

        private static KeyValuePair<string, ExpectedRange<decimal>>[] GetRespiratoryRateRangesByUnitOfMeasure(string unitOfMeasure)
        {
            return unitOfMeasure switch
            {
                UnitsOfMeasureConstants.RepetitionsPerMinuteUnitOfMeasure => CreateNormalRange(12M, 20M),
                _ => null
            };
        }

        private static KeyValuePair<string, ExpectedRange<decimal>>[] CreateNormalRange(decimal min, decimal max)
        {
            return new KeyValuePair<string, ExpectedRange<decimal>>[]
            {
                new(Normal, new ExpectedRange<decimal>()
                {
                    Max = max,
                    Min = min
                })
            };
        }
    }
}