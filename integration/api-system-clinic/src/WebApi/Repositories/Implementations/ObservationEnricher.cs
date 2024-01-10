// <copyright file="ObservationEnricher.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Duly.Clinic.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IObservationEnricher"/>
    /// </summary>
    internal class ObservationEnricher : IObservationEnricher
    {
        private readonly IEnrichBMI _bmiEnricher;

        public ObservationEnricher(IEnrichBMI bmiEnricher)
        {
            _bmiEnricher = bmiEnricher;
        }

        public void EnrichResults(IEnumerable<Observation> observationsToEnrich, IEnumerable<Observation> complimentaryObservations)
        {
            EnrichBMI(observationsToEnrich, complimentaryObservations);
        }

        private void EnrichBMI(IEnumerable<Observation> observationsToEnrich, IEnumerable<Observation> complimentaryObservations)
        {
            var bmiObservation = observationsToEnrich.FirstOrDefault(x => x.Type == ObservationType.BodyMassIndex);

            if (bmiObservation is null)
            {
                return;
            }

            _bmiEnricher.EnrichBMI(bmiObservation, complimentaryObservations);
        }
    }
}
