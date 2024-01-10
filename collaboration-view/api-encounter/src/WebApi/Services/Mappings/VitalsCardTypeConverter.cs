// <copyright file="VitalsCardTypeConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    internal class VitalsCardTypeConverter : ITypeConverter<ObservationType, VitalsCardType>
    {
        public VitalsCardType Convert(ObservationType source, VitalsCardType destination, ResolutionContext context)
        {
            return source switch
            {
                ObservationType.BloodPressure => VitalsCardType.BloodPressure,
                ObservationType.BodyMassIndex => VitalsCardType.BodyMassIndex,
                ObservationType.BodyTemperature => VitalsCardType.Temperature,
                ObservationType.OxygenSaturation => VitalsCardType.BloodOxygen,
                ObservationType.HeartRate => VitalsCardType.HeartRate,
                ObservationType.RespiratoryRate => VitalsCardType.RespiratoryRate,
                ObservationType.PainLevel => VitalsCardType.PainLevel,
                ObservationType.BodyWeight => VitalsCardType.WeightAndHeight,
                ObservationType.BodyHeight => VitalsCardType.WeightAndHeight,
                _ => throw new ServiceNotMappedException("Could not map ObservationType to VitalsCardType")
            };
        }
    }
}