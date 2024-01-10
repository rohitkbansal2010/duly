// <copyright file="ObservationConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    internal class ObservationConverter : ITypeConverter<Observation, VitalsCard>
    {
        public VitalsCard Convert(Observation source, VitalsCard destination, ResolutionContext context)
        {
            return new VitalsCard
            {
                CardType = context.Mapper.Map<VitalsCardType>(source.Type),
                Vitals = ConvertVital(source, context.Mapper),
            };
        }

        private static Vital[] ConvertVital(Observation source, IRuntimeMapper contextMapper)
        {
            if (source.Type == ObservationType.BodyMassIndex)
            {
                return BuildBodyMassIndex(source, contextMapper).ToArray();
            }

            return new[]
            {
                BuildVital(source, contextMapper)
            };
        }

        private static decimal? TryGetMaxScaleValue(ObservationType observationType)
        {
            if (observationType == ObservationType.PainLevel)
                return 10;

            return null;
        }

        private static VitalMeasurementType ConvertVitalMeasurementType(ObservationType observationType, ObservationComponentType? observationComponentType, IRuntimeMapper contextMapper)
        {
            return observationComponentType.HasValue ?
                contextMapper.Map<VitalMeasurementType>(observationComponentType.Value) :
                contextMapper.Map<VitalMeasurementType>(observationType);
        }

        private static IEnumerable<Vital> BuildBodyMassIndex(Observation source, IRuntimeMapper contextMapper)
        {
            foreach (var component in source.Components.GroupBy(component => component.Type))
            {
                switch (component.Key)
                {
                    case null:
                        yield return BuildVital(source.Type, component, source.Date, contextMapper);
                        break;
                    case ObservationComponentType.Weight:
                        yield return BuildVital(ObservationType.BodyWeight, component, source.Date, contextMapper);
                        break;
                    case ObservationComponentType.Height:
                        yield return BuildVital(ObservationType.BodyHeight, component, source.Date, contextMapper);
                        break;
                }
            }
        }

        private static Vital BuildVital(Observation source, IRuntimeMapper contextMapper)
        {
            return BuildVital(source.Type, source.Components, source.Date, contextMapper);
        }

        private static Vital BuildVital(ObservationType observationType, IEnumerable<ObservationComponent> components, DateTimeOffset measured, IRuntimeMapper contextMapper)
        {
            return new Vital
            {
                VitalType = contextMapper.Map<VitalType>(observationType),
                Measurements = components.SelectMany(
                    component => component.Measurements, (component, measurement) =>
                        new VitalMeasurement
                        {
                            Measured = measured,
                            Value = measurement.Value,
                            Unit = measurement.Unit,
                            MaxScaleValue = TryGetMaxScaleValue(observationType),
                            MeasurementType = ConvertVitalMeasurementType(observationType, component.Type, contextMapper)
                        }).ToArray()
            };
        }
    }
}