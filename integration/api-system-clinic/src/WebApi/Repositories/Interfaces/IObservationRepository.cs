// <copyright file="IObservationRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Observation"/> entities.
    /// </summary>
    public interface IObservationRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="Observation"/> which match with the filter.
        /// </summary>
        /// <param name="patientId">Identifier of Patient.</param>
        /// <param name="observationTypes">Types of the observation which should be included.</param>
        /// /// <param name="lowerBoundDate">Lower time boundary.</param>
        /// /// <param name="upperBoundBoundDate">Upper time boundary.</param>
        /// <returns>Filtered items of <see cref="Observation"/>.</returns>
        Task<IEnumerable<Observation>> FindObservationsForPatientAsync(string patientId, ObservationType[] observationTypes, System.DateTime lowerBoundDate, System.DateTime upperBoundBoundDate);

        /// <summary>
        /// Retrieve all last items of <see cref="Observation"/> which match with the filter.
        /// </summary>
        /// <param name="patientId">Identifier of Patient.</param>
        /// <param name="observationTypes">Types of the observation which should be included.</param>
        /// <returns>Filtered items of <see cref="Observation"/>.</returns>
        Task<IEnumerable<Observation>> FindLastObservationsForPatientAsync(string patientId, ObservationType[] observationTypes);
    }
}
