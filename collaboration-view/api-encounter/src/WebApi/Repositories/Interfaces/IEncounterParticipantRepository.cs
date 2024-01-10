// <copyright file="IEncounterParticipantRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Models.Participant"/> entities.
    /// </summary>
    internal interface IEncounterParticipantRepository
    {
        /// <summary>
        /// Gets <see cref="Models.Participant"/> based on encounterId.
        /// </summary>
        /// <param name="encounterId"> Id of encounter.</param>
        /// <returns>Enumerable of <see cref="Models.Participant"/>.</returns>
        public Task<IEnumerable<Models.Participant>> GetParticipantsByEncounterId(string encounterId);
    }
}