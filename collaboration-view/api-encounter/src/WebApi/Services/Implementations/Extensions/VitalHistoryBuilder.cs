// <copyright file="VitalHistoryBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using System;
using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations.Extensions
{
    internal static class VitalHistoryBuilder
    {
        public static VitalHistory BuildVitalHistory(VitalsCardType vitalsCardType, Observation[] observations)
        {
            var chartDatasets = BuildChartDatasets(vitalsCardType, observations, out var unitOfMeasure);
            return new VitalHistory
            {
                Chart = new Chart<DateTimeOffset, decimal>
                {
                    ChartOptions = BuildChartOptions(vitalsCardType, unitOfMeasure),
                    Datasets = chartDatasets
                }
            };
        }

        private static ChartOptions<DateTimeOffset, decimal> BuildChartOptions(VitalsCardType vitalsCardType, string unitOfMeasure)
        {
            return new ChartOptions<DateTimeOffset, decimal>
            {
                ChartScales = new ChartScales<DateTimeOffset, decimal>
                {
                    YAxis = vitalsCardType.BuildYAxis(unitOfMeasure)
                }
            };
        }

        private static ChartDataset<DateTimeOffset, decimal>[] BuildChartDatasets(VitalsCardType vitalsCardType, IReadOnlyList<Observation> observations, out string dimension)
        {
            return vitalsCardType switch
            {
                VitalsCardType.BloodPressure => new[]
                {
                    BuildChartDataset(observations, vitalsCardType, out dimension, componentType: ObservationComponentType.Systolic),
                    BuildChartDataset(observations, vitalsCardType, out _, componentType: ObservationComponentType.Diastolic)
                },
                VitalsCardType.Temperature => new[]
                {
                    BuildChartDataset(observations, vitalsCardType, out dimension, VitalPreferredUnitOfMeasure.PreferredTemperatureUnitOfMeasure)
                },
                VitalsCardType.WeightAndHeight => new[]
                {
                    BuildChartDataset(observations, vitalsCardType, out dimension, VitalPreferredUnitOfMeasure.PreferredWeightUnitOfMeasure),
                },
                _ => new[] { BuildChartDataset(observations, vitalsCardType, out dimension) }
            };
        }

        private static ChartDataset<DateTimeOffset, decimal> BuildChartDataset(
            IReadOnlyList<Observation> observations,
            VitalsCardType vitalsCardType,
            out string dimension,
            string preferredUnitOfMeasure = null,
            ObservationComponentType? componentType = null)
        {
            var chartData = BuildChartData(observations, preferredUnitOfMeasure, componentType, out var label, out var visible);
            var ranges = vitalsCardType.BuildExpectedRanges(chartData.Dimension, label);
            dimension = chartData.Dimension;
            return new ChartDataset<DateTimeOffset, decimal>
            {
                Label = label,
                Data = chartData,
                Ranges = ranges,
                Visible = visible
            };
        }

        private static ChartData<DateTimeOffset, decimal> BuildChartData(
            IReadOnlyList<Observation> observations,
            string preferredUnitOfMeasure,
            ObservationComponentType? componentType,
            out string label,
            out bool visible)
        {
            label = null;
            string dimension = null;
            visible = true;
            var chartDataValues = new ChartDataValue<DateTimeOffset, decimal>[observations.Count];
            for (var i = 0; i < observations.Count; i++)
            {
                var observation = observations[i];
                var component = VitalPreferredUnitOfMeasure.SelectComponent(observation, componentType);
                var measurement = VitalPreferredUnitOfMeasure.SelectMeasurement(component, preferredUnitOfMeasure);
                chartDataValues[i] = new ChartDataValue<DateTimeOffset, decimal>
                {
                    X = observation.Date,
                    Y = measurement.Value
                };

                if (i != 0)
                {
                    continue;
                }

                dimension = measurement.Unit;
                var observationType = observation.Type;
                label = VitalPreferredUnitOfMeasure.ChooseLabel(component, observationType);
                visible = observationType.BuildVisible();
            }

            var chartData = new ChartData<DateTimeOffset, decimal>
            {
                Dimension = dimension,
                Values = chartDataValues
            };

            return chartData;
        }
    }
}