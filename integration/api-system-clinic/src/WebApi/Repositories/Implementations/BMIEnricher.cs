// <copyright file="BMIEnricher.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Duly.Clinic.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IEnrichBMI"/>
    /// </summary>
    internal class BMIEnricher : IEnrichBMI
    {
        public void EnrichBMI(Observation bmiObservation, IEnumerable<Observation> complimentaryObservations)
        {
            var observations = complimentaryObservations.ToArray();
            var height = FindMatchingObservation(observations, ObservationType.BodyHeight, bmiObservation.Date.Date);
            var weight = FindMatchingObservation(observations, ObservationType.BodyWeight, bmiObservation.Date.Date);

            if (height is null || weight is null)
            {
                return;
            }

            var heightComponents = ExtractComponents(ObservationComponentType.Weight, weight.Components).ToArray();
            var weightComponents = ExtractComponents(ObservationComponentType.Height, height.Components).ToArray();

            if (heightComponents.Any() && weightComponents.Any())
            {
                bmiObservation.Components = bmiObservation.Components.
                    Concat(heightComponents).
                    Concat(weightComponents).ToArray();
            }
        }

        private static IEnumerable<ObservationComponent> ExtractComponents( ObservationComponentType type, IEnumerable<ObservationComponent> components)
        {
            return components
                .Where(x => !x.Type.HasValue)
                .Select(component => new ObservationComponent
                {
                    Type = type,
                    Measurements = component.Measurements
                });
        }

        private static Observation FindMatchingObservation(IEnumerable<Observation> systemObservations, ObservationType type, DateTime date)
        {
            return systemObservations
                .Where(x =>
                    x.Date.Date == date &&
                    x.Type == type)
                .OrderByDescending(x => x.Date)
                .FirstOrDefault();
        }
    }
}
