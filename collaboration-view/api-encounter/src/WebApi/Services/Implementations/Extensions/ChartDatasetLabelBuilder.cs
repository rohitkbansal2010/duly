// <copyright file="ChartDatasetLabelBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Constants;
using System;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations.Extensions
{
    internal static class ChartDatasetLabelBuilder
    {
        public static string BuildChartDatasetLabel(this ObservationType observationType)
        {
            return observationType switch
            {
                ObservationType.OxygenSaturation => "Blood Oxygen",
                ObservationType.HeartRate => "Heart Rate",
                ObservationType.RespiratoryRate => "Respiratory Rate",
                ObservationType.PainLevel => "Pain Level",
                ObservationType.BodyTemperature => "Temperature",
                ObservationType.BodyWeight => "Weight",
                ObservationType.BodyHeight => "Height",
                ObservationType.BodyMassIndex => "BMI",
                _ => throw new ArgumentOutOfRangeException(nameof(observationType), observationType, null)
            };
        }

        public static string BuildChartDatasetLabel(this ObservationComponentType observationComponentType)
        {
            return observationComponentType switch
            {
                ObservationComponentType.Systolic => VitalsConstants.SystolicBloodPressureLabel,
                ObservationComponentType.Diastolic => VitalsConstants.DiastolicBloodPressureLabel,
                _ => throw new ArgumentOutOfRangeException(nameof(observationComponentType), observationComponentType, null)
            };
        }
    }
}