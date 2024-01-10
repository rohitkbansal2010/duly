// <copyright file="ICareTeamParticipantRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="CareTeamParticipant"/> entities.
    /// </summary>
    public interface ICareTeamParticipantRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="CareTeamParticipant"/> which match with the filter.
        /// </summary>
        /// <param name="encounterId">Identifier of encounter.</param>
        /// <param name="status">Status of CareTeam.</param>
        /// <param name="category">Category of CareTeam member.</param>
        /// <returns>Filtered items of <see cref="CareTeamParticipant"/>.</returns>
        Task<IEnumerable<CareTeamParticipant>> GetCareTeamsParticipantsByEncounterIdAsync(string encounterId, CareTeamStatus status, CareTeamCategory category);

        /// <summary>
        /// Retrieve all items of <see cref="CareTeamParticipant"/> which match with the filter.
        /// </summary>
        /// <param name="patientId">Identifier of patient.</param>
        /// <param name="status">Status of CareTeam.</param>
        /// <param name="category">Category of CareTeam member.</param>
        /// <returns>Filtered items of <see cref="CareTeamParticipant"/>.</returns>
        Task<IEnumerable<CareTeamParticipant>> GetCareTeamsParticipantsByPatientIdAsync(string patientId, CareTeamStatus status, CareTeamCategory category);
    }
}