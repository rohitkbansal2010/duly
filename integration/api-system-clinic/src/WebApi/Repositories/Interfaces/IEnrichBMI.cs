// <copyright file="IEnrichBMI.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using System.Collections.Generic;

namespace Duly.Clinic.Api.Repositories.Interfaces
{
    /// <summary>
    /// Enriches BMI observation.
    /// </summary>
    internal interface IEnrichBMI
    {
        /// <summary>
        /// Adds Weight and Height to BMI measurement if they can be deduced from Observations of that day.
        /// </summary>
        /// <param name="bmiObservation">Observation to enrich.</param>
        /// <param name="complimentaryObservations">Observations that can be used to get extra data.</param>
        void EnrichBMI(Observation bmiObservation, IEnumerable<Observation> complimentaryObservations);
    }
}
