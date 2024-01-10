// <copyright file="VitalMeasurementTypeConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    internal class VitalMeasurementTypeConverter :
        ITypeConverter<ObservationComponentType, VitalMeasurementType>,
        ITypeConverter<ObservationType, VitalMeasurementType>
    {
        public VitalMeasurementType Convert(ObservationComponentType source, VitalMeasurementType destination, ResolutionContext context)
        {
            return source switch
            {
                ObservationComponentType.Systolic => VitalMeasurementType.SystolicBloodPressure,
                ObservationComponentType.Diastolic => VitalMeasurementType.DiastolicBloodPressure,
                ObservationComponentType.Height => VitalMeasurementType.BodyHeight,
                ObservationComponentType.Weight => VitalMeasurementType.BodyWeight,
                _ => throw new ServiceNotMappedException("Could not map ObservationComponentType to VitalMeasurementType")
            };
        }

        public VitalMeasurementType Convert(ObservationType source, VitalMeasurementType destination, ResolutionContext context)
        {
            return source switch
            {
                ObservationType.OxygenSaturation => VitalMeasurementType.OxygenSaturation,
                ObservationType.HeartRate => VitalMeasurementType.HeartRate,
                ObservationType.RespiratoryRate => VitalMeasurementType.RespiratoryRate,
                ObservationType.PainLevel => VitalMeasurementType.PainLevel,
                ObservationType.BodyTemperature => VitalMeasurementType.BodyTemperature,
                ObservationType.BodyWeight => VitalMeasurementType.BodyWeight,
                ObservationType.BodyHeight => VitalMeasurementType.BodyHeight,
                ObservationType.BodyMassIndex => VitalMeasurementType.BodyMassIndex,
                _ => throw new ServiceNotMappedException("Could not map ObservationType to VitalMeasurementType")
            };
        }
    }
}