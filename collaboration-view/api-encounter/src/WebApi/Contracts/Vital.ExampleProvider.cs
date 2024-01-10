// -----------------------------------------------------------------------
// <copyright file="Vital.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Encounter.Api.Services.Constants;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class VitalExampleProvider : IExamplesProvider<Vital>
    {
        public Vital GetExamples()
        {
            return BuildExampleFor(VitalType.BloodPressure);
        }

        public Vital BuildExampleFor(VitalType vitalType)
        {
            return vitalType switch
            {
                VitalType.BloodPressure => BuildBloodPressureExample(vitalType),
                VitalType.BloodOxygen => BuildBloodOxygenExample(vitalType),
                VitalType.HeartRate => BuildHeartRateExample(vitalType),
                VitalType.RespiratoryRate => BuildRespiratoryRateExample(vitalType),
                VitalType.PainLevel => BuildPainLevelExample(vitalType),
                VitalType.Temperature => BuildTemperatureExample(vitalType),
                VitalType.Weight => BuildWeightExample(vitalType),
                VitalType.Height => BuildHeightExample(vitalType),
                VitalType.BodyMassIndex => BuildBodyMassIndexExample(vitalType),
                _ => BuildBloodPressureExample(vitalType),
            };
        }

        private static Vital BuildBloodPressureExample(VitalType vitalType)
        {
            var measured = DateTime.Now;

            return new Vital
            {
                VitalType = vitalType,
                Measurements = new[]
                {
                    new VitalMeasurement
                    {
                        MeasurementType = VitalMeasurementType.SystolicBloodPressure,
                        Value = 135.14M,
                        Measured = new DateTimeOffset(measured),
                        Unit = UnitsOfMeasureConstants.PercentUnitOfMeasure
                    },
                    new VitalMeasurement
                    {
                        MeasurementType = VitalMeasurementType.DiastolicBloodPressure,
                        Value = 93.07M,
                        Measured = new DateTimeOffset(measured),
                        Unit = UnitsOfMeasureConstants.PercentUnitOfMeasure
                    },
                }
            };
        }

        private static Vital BuildBloodOxygenExample(VitalType vitalType)
        {
            var measured = DateTime.Now;

            return new Vital
            {
                VitalType = vitalType,
                Measurements = new[]
                {
                    new VitalMeasurement
                    {
                        MeasurementType = VitalMeasurementType.OxygenSaturation,
                        Value = 92.31M,
                        Measured = new DateTimeOffset(measured),
                        Unit = UnitsOfMeasureConstants.PercentUnitOfMeasure
                    }
                }
            };
        }

        private static Vital BuildHeartRateExample(VitalType vitalType)
        {
            var measured = DateTime.Now;

            return new Vital
            {
                VitalType = vitalType,
                Measurements = new[]
                {
                    new VitalMeasurement
                    {
                        MeasurementType = VitalMeasurementType.HeartRate,
                        Value = 81,
                        Measured = new DateTimeOffset(measured),
                        Unit = UnitsOfMeasureConstants.RepetitionsPerMinuteUnitOfMeasure
                    }
                }
            };
        }

        private static Vital BuildRespiratoryRateExample(VitalType vitalType)
        {
            var measured = DateTime.Now;

            return new Vital
            {
                VitalType = vitalType,
                Measurements = new[]
                {
                    new VitalMeasurement
                    {
                        MeasurementType = VitalMeasurementType.RespiratoryRate,
                        Value = 14,
                        Measured = new DateTimeOffset(measured),
                        Unit = UnitsOfMeasureConstants.RepetitionsPerMinuteUnitOfMeasure
                    }
                }
            };
        }

        private static Vital BuildPainLevelExample(VitalType vitalType)
        {
            var measured = DateTime.Now;

            return new Vital
            {
                VitalType = vitalType,
                Measurements = new[]
                {
                    new VitalMeasurement
                    {
                        MeasurementType = VitalMeasurementType.PainLevel,
                        Value = 3,
                        Measured = new DateTimeOffset(measured),
                        MaxScaleValue = 10
                    }
                }
            };
        }

        private static Vital BuildTemperatureExample(VitalType vitalType)
        {
            var measured = DateTime.Now;

            return new Vital
            {
                VitalType = vitalType,
                Measurements = new[]
                {
                    new VitalMeasurement
                    {
                        MeasurementType = VitalMeasurementType.BodyTemperature,
                        Value = 36.71M,
                        Measured = new DateTimeOffset(measured),
                        Unit = "Cel"
                    }
                }
            };
        }

        private static Vital BuildWeightExample(VitalType vitalType)
        {
            var measured = DateTime.Now;

            return new Vital
            {
                VitalType = vitalType,
                Measurements = new[]
                {
                    new VitalMeasurement
                    {
                        MeasurementType = VitalMeasurementType.BodyWeight,
                        Value = 65.28M,
                        Measured = new DateTimeOffset(measured),
                        Unit = "kg"
                    }
                }
            };
        }

        private static Vital BuildHeightExample(VitalType vitalType)
        {
            var measured = DateTime.Now;

            return new Vital
            {
                VitalType = vitalType,
                Measurements = new[]
                {
                    new VitalMeasurement
                    {
                        MeasurementType = VitalMeasurementType.BodyHeight,
                        Value = 175.29M,
                        Measured = new DateTimeOffset(measured),
                        Unit = "cm"
                    }
                }
            };
        }

        private static Vital BuildBodyMassIndexExample(VitalType vitalType)
        {
            var measured = DateTime.Now;

            return new Vital
            {
                VitalType = vitalType,
                Measurements = new[]
                {
                    new VitalMeasurement
                    {
                        MeasurementType = VitalMeasurementType.BodyMassIndex,
                        Value = 21.27M,
                        Measured = new DateTimeOffset(measured),
                        Unit = "kg/m2"
                    }
                }
            };
        }
    }
}