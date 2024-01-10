// <copyright file="IObservationEnricher.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using System.Collections.Generic;

namespace Duly.Clinic.Api.Repositories.Interfaces
{
    /// <summary>
    /// Adds data to Observations.
    /// </summary>
    internal interface IObservationEnricher
    {
        /// <summary>
        /// Adds data to <paramref name="observationsToEnrich"/> from <paramref name="complimentaryObservations"/>.
        /// </summary>
        /// <param name="observationsToEnrich">Observations that need data added to them.</param>
        /// <param name="complimentaryObservations">Observations that contain other data.</param>
        void EnrichResults(IEnumerable<Observation> observationsToEnrich, IEnumerable<Observation> complimentaryObservations);
    }
}
