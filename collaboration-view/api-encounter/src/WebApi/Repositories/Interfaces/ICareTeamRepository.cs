// <copyright file="ICareTeamRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Models.CareTeamParticipant"/> entities.
    /// </summary>
    internal interface ICareTeamRepository
    {
        /// <summary>
        /// Retrieves <see cref="Models.CareTeamParticipant"/> based on ID of patient.
        /// </summary>
        /// <param name="patientId">Id of patient.</param>
        /// <param name="status">Status of CareTeam.</param>
        /// <param name="category">Category of CareTeam.</param>
        /// <returns>Enumerable of CareTeam participants.</returns>
        public Task<IEnumerable<Models.CareTeamParticipant>> GetCareTeamParticipantsByPatientIdAsync(string patientId, CareTeamStatus status, CareTeamCategory category);
    }
}