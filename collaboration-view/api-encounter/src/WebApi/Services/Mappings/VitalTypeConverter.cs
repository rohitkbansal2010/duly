// <copyright file="VitalTypeConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    internal class VitalTypeConverter : ITypeConverter<ObservationType, VitalType>
    {
        public VitalType Convert(ObservationType source, VitalType destination, ResolutionContext context)
        {
            return source switch
            {
                ObservationType.BloodPressure => VitalType.BloodPressure,
                ObservationType.BodyMassIndex => VitalType.BodyMassIndex,
                ObservationType.BodyTemperature => VitalType.Temperature,
                ObservationType.OxygenSaturation => VitalType.BloodOxygen,
                ObservationType.HeartRate => VitalType.HeartRate,
                ObservationType.RespiratoryRate => VitalType.RespiratoryRate,
                ObservationType.PainLevel => VitalType.PainLevel,
                ObservationType.BodyWeight => VitalType.Weight,
                ObservationType.BodyHeight => VitalType.Height,
                _ => throw new ServiceNotMappedException("Could not map ObservationType to VitalType")
            };
        }
    }
}