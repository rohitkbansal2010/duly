// <copyright file="IObservationRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Models.Observation"/> entities.
    /// </summary>
    internal interface IObservationRepository
    {
        /// <summary>
        /// Gets the latest patient's <see cref="Models.Observation"/> of the <see cref="ObservationType"/>.
        /// If they exist, otherwise they don't preserve in the result.
        /// </summary>
        /// <param name="patientId">Id of the patient.</param>
        /// <param name="observationTypes">Arrays of the required types to consume.</param>
        /// <returns>Enumerable of <see cref="Models.Observation"/>.</returns>
        Task<IEnumerable<Models.Observation>> GetLatestObservationsForPatientAsync(string patientId, Models.ObservationType[] observationTypes);

        /// <summary>
        /// Gets patient's <see cref="Models.Observation"/> of the <see cref="ObservationType"/>.
        /// If they exist, otherwise they don't preserve in the result.
        /// </summary>
        /// <param name="patientId">Id of the patient.</param>
        /// <param name="lowerBoundDate">A lower bound of observation's effective date as DateTimeOffset UTC+0.</param>
        /// <param name="upperBoundDate">An upper bound of observation's effective date as DateTimeOffset UTC+0.</param>
        /// <param name="observationTypes">Arrays of the required types to consume.</param>
        /// <returns>Enumerable of <see cref="Models.Observation"/>.</returns>
        Task<IEnumerable<Models.Observation>> GetObservationsForPatientAsync(string patientId, DateTimeOffset lowerBoundDate, DateTimeOffset upperBoundDate, Models.ObservationType[] observationTypes);
    }
}