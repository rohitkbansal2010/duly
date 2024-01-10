// -----------------------------------------------------------------------
// <copyright file="VitalsCard.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Swashbuckle.AspNetCore.Filters;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class VitalsCardExampleProvider : IExamplesProvider<VitalsCard>
    {
        public VitalsCard GetExamples()
        {
            return BuildExampleFor(VitalsCardType.BloodPressure);
        }

        public VitalsCard BuildExampleFor(VitalsCardType vitalsCardType)
        {
            return vitalsCardType switch
            {
                VitalsCardType.BloodPressure => BuildBloodPressureExample(vitalsCardType),
                VitalsCardType.BloodOxygen => BuildBloodOxygenExample(vitalsCardType),
                VitalsCardType.HeartRate => BuildHeartRateExample(vitalsCardType),
                VitalsCardType.RespiratoryRate => BuildRespiratoryRateExample(vitalsCardType),
                VitalsCardType.PainLevel => BuildPainLevelExample(vitalsCardType),
                VitalsCardType.Temperature => BuildTemperatureExample(vitalsCardType),
                VitalsCardType.WeightAndHeight => BuildWeightAndHeightExample(vitalsCardType),
                VitalsCardType.BodyMassIndex => BuildBodyMassIndexExample(vitalsCardType),
                _ => BuildBloodPressureExample(vitalsCardType)
            };
        }

        private static VitalsCard BuildBloodPressureExample(VitalsCardType cardType)
        {
            return new VitalsCard
            {
                CardType = cardType,
                Vitals = new[]
                {
                    new VitalExampleProvider().BuildExampleFor(VitalType.BloodPressure)
                }
            };
        }

        private static VitalsCard BuildBloodOxygenExample(VitalsCardType cardType)
        {
            return new VitalsCard
            {
                CardType = cardType,
                Vitals = new[]
                {
                    new VitalExampleProvider().BuildExampleFor(VitalType.BloodOxygen)
                }
            };
        }

        private static VitalsCard BuildHeartRateExample(VitalsCardType cardType)
        {
            return new VitalsCard
            {
                CardType = cardType,
                Vitals = new[]
                {
                    new VitalExampleProvider().BuildExampleFor(VitalType.HeartRate)
                }
            };
        }

        private static VitalsCard BuildRespiratoryRateExample(VitalsCardType cardType)
        {
            return new VitalsCard
            {
                CardType = cardType,
                Vitals = new[]
                {
                    new VitalExampleProvider().BuildExampleFor(VitalType.RespiratoryRate)
                }
            };
        }

        private static VitalsCard BuildPainLevelExample(VitalsCardType cardType)
        {
            return new VitalsCard
            {
                CardType = cardType,
                Vitals = new[]
                {
                    new VitalExampleProvider().BuildExampleFor(VitalType.PainLevel)
                }
            };
        }

        private static VitalsCard BuildTemperatureExample(VitalsCardType cardType)
        {
            return new VitalsCard
            {
                CardType = cardType,
                Vitals = new[]
                {
                    new VitalExampleProvider().BuildExampleFor(VitalType.Temperature)
                }
            };
        }

        private static VitalsCard BuildWeightAndHeightExample(VitalsCardType cardType)
        {
            var provider = new VitalExampleProvider();

            return new VitalsCard
            {
                CardType = cardType,
                Vitals = new[]
                {
                    provider.BuildExampleFor(VitalType.Weight),
                    provider.BuildExampleFor(VitalType.Height)
                }
            };
        }

        private static VitalsCard BuildBodyMassIndexExample(VitalsCardType cardType)
        {
            return new VitalsCard
            {
                CardType = cardType,
                Vitals = new[]
                {
                    new VitalExampleProvider().BuildExampleFor(VitalType.BodyMassIndex)
                }
            };
        }
    }
}