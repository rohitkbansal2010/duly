// <copyright file="VitalPreferredUnitOfMeasure.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Services.Constants;
using System;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations.Extensions
{
    public static class VitalPreferredUnitOfMeasure
    {
        public const string PreferredWeightUnitOfMeasure = "lb_av";
        public const string PreferredHeightUnitOfMeasure = "in_i";

        public const string PreferredTemperatureUnitOfMeasure =
            UnitsOfMeasureConstants.FahrenheitTemperatureUnitOfMeasure;

        public static void ChooseTemperatureUnitOfMeasure(this Vital vital)
        {
            var index = FindMeasurementUnitIndex(vital, PreferredTemperatureUnitOfMeasure);
            vital.Measurements = new[] { vital.Measurements[index] };
        }

        public static void ChooseWeightUnitOfMeasure(this Vital vital)
        {
            var index = FindMeasurementUnitIndex(vital, PreferredWeightUnitOfMeasure);
            vital.Measurements = new[] { vital.Measurements[index] };
        }

        public static void ChooseHeightUnitOfMeasure(this Vital vital)
        {
            var index = FindMeasurementUnitIndex(vital, PreferredHeightUnitOfMeasure);
            vital.Measurements = new[] { vital.Measurements[index] };
        }

        internal static Repositories.Models.ObservationMeasurement SelectMeasurement(Repositories.Models.ObservationComponent observationComponent, string preferredUnitOfMeasure)
        {
            var measurements = observationComponent.Measurements;

            if (string.IsNullOrEmpty(preferredUnitOfMeasure))
            {
                return measurements[0];
            }

            if (measurements.Length < 2)
            {
                return measurements[0];
            }

            var index = Array.FindIndex(measurements, measurement => measurement.Unit == preferredUnitOfMeasure);
            if (index == -1)
            {
                index = 0;
            }

            return measurements[index];
        }

        internal static Repositories.Models.ObservationComponent SelectComponent(Repositories.Models.Observation observation, Repositories.Models.ObservationComponentType? componentType)
        {
            var index = Array.FindIndex(observation.Components, observationComponent => observationComponent.Type == componentType);
            if (index == -1)
            {
                index = 0;
            }

            return observation.Components[index];
        }

        internal static string ChooseLabel(Repositories.Models.ObservationComponent component, Repositories.Models.ObservationType observationType)
        {
            var observationComponentType = component.Type;
            return observationComponentType.HasValue
                ? observationComponentType.Value.BuildChartDatasetLabel()
                : observationType.BuildChartDatasetLabel();
        }

        private static int FindMeasurementUnitIndex(Vital vital, string requiredUnit)
        {
            var index = Array.FindIndex(vital.Measurements, measurement => measurement.Unit == requiredUnit);
            if (index == -1)
            {
                index = 0;
            }

            return index;
        }
    }
}
