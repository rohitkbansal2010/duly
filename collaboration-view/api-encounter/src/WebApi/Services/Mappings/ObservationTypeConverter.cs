// <copyright file="ObservationTypeConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    internal class ObservationTypeConverter : ITypeConverter<VitalsCardType, ObservationType[]>
    {
        public ObservationType[] Convert(VitalsCardType source, ObservationType[] destination, ResolutionContext context)
        {
            return source switch
            {
                VitalsCardType.BloodPressure => new[] { ObservationType.BloodPressure },
                VitalsCardType.BloodOxygen => new[] { ObservationType.OxygenSaturation },
                VitalsCardType.HeartRate => new[] { ObservationType.HeartRate },
                VitalsCardType.RespiratoryRate => new[] { ObservationType.RespiratoryRate },
                VitalsCardType.PainLevel => new[] { ObservationType.PainLevel },
                VitalsCardType.Temperature => new[] { ObservationType.BodyTemperature },
                VitalsCardType.WeightAndHeight => new[] { ObservationType.BodyWeight },
                VitalsCardType.BodyMassIndex => new[] { ObservationType.BodyMassIndex },
                _ => throw new ServiceNotMappedException("Could not map VitalsCardType to ObservationType")
            };
        }
    }
}