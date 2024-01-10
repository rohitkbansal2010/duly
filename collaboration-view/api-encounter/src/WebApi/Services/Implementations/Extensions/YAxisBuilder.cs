// <copyright file="YAxisBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using System;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations.Extensions
{
    internal static class YAxisBuilder
    {
        public static Axis<decimal> BuildYAxis(this VitalsCardType vitalsCardType, string unitOfMeasure)
        {
            return vitalsCardType switch
            {
                VitalsCardType.BloodPressure => BuildBloodPressureChartScale(),
                VitalsCardType.BloodOxygen => BuildBloodOxygenChartScale(),
                VitalsCardType.HeartRate => BuildHeartRateChartScale(),
                VitalsCardType.Temperature => BuildTemperatureChartScale(unitOfMeasure),
                VitalsCardType.RespiratoryRate => BuildRespiratoryRateChartScale(),
                VitalsCardType.PainLevel => BuildPainLevelChartScale(),
                VitalsCardType.WeightAndHeight => BuildWeightAndHeightChartScale(unitOfMeasure),
                VitalsCardType.BodyMassIndex => BuildBodyMassIndexChartScale(),
                _ => throw new ArgumentOutOfRangeException(nameof(vitalsCardType), vitalsCardType, null)
            };
        }

        private static Axis<decimal> BuildBloodPressureChartScale()
        {
            //Blood Pressure Chart
            //Y - axis: The range shall be - [0;240] Range step shall be 40
            return new Axis<decimal>
            {
                Max = 240,
                Min = 0,
                StepSize = 40
            };
        }

        private static Axis<decimal> BuildBloodOxygenChartScale()
        {
            //Blood Oxygen Chart
            //Y - axis: The range shall be - [80; 100] Range step shall be 4
            return new Axis<decimal>
            {
                Max = 100,
                Min = 80,
                StepSize = 4
            };
        }

        private static Axis<decimal> BuildHeartRateChartScale()
        {
            //Heart Rate Chart
            //Y - axis: The range shall be - [0; 200] Range step shall be 40
            return new Axis<decimal>
            {
                Max = 200,
                Min = 0,
                StepSize = 40
            };
        }

        private static Axis<decimal> BuildTemperatureChartScale(string unitOfMeasure)
        {
            //Temperature Chart
            //Based on this unit the system shall scale Y axis
            if (unitOfMeasure == VitalPreferredUnitOfMeasure.PreferredTemperatureUnitOfMeasure)
            {
                //Y - axis: The range shall be - [90; 120] Range step shall be 5
                return new Axis<decimal>
                {
                    Max = 120,
                    Min = 90,
                    StepSize = 5
                };
            }

            return new Axis<decimal>
            {
                //Y - axis: The range shall be - [34; 42] Range step shall be 2
                Max = 42,
                Min = 34,
                StepSize = 2
            };
        }

        private static Axis<decimal> BuildRespiratoryRateChartScale()
        {
            //Respiratory Rate Chart
            //Y - axis: The range shall be - [0; 60] Range step shall be 10
            return new Axis<decimal>
            {
                Max = 60,
                Min = 0,
                StepSize = 10
            };
        }

        private static Axis<decimal> BuildPainLevelChartScale()
        {
            //Pain Level Chart
            //Y - axis: The range shall be [1; 10] Range step shall be 2
            return new Axis<decimal>
            {
                Max = 10,
                Min = 1,
                StepSize = 2
            };
        }

        private static Axis<decimal> BuildWeightAndHeightChartScale(string unitOfMeasure)
        {
            //Weight & Height Chart
            //Based on this unit the system shall scale Y axis
            if (unitOfMeasure == VitalPreferredUnitOfMeasure.PreferredWeightUnitOfMeasure)
            {
                //Y - axis: The range shall be - [80; 480] Range step shall be 50
                return new Axis<decimal>
                {
                    Max = 480,
                    Min = 80,
                    StepSize = 50
                };
            }

            //Y - axis: The range shall be - [30; 210] Range step shall be 30
            return new Axis<decimal>
            {
                Max = 210,
                Min = 30,
                StepSize = 30
            };
        }

        private static Axis<decimal> BuildBodyMassIndexChartScale()
        {
            //BMI (body mass index) Chart
            //Y - axis: The range shall be - [13; 83] Range step shall be 14
            return new Axis<decimal>
            {
                Max = 83,
                Min = 13,
                StepSize = 14
            };
        }
    }
}