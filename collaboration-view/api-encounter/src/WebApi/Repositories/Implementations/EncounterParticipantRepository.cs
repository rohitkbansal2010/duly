// -----------------------------------------------------------------------
// <copyright file="EncounterParticipantRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using AutoMapper;
using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IEncounterParticipantRepository"/>
    /// </summary>
    internal class EncounterParticipantRepository : IEncounterParticipantRepository
    {
        private readonly IEncounterContext _encounterContext;
        private readonly IEncountersClient _encountersClient;
        private readonly IMapper _mapper;

        public EncounterParticipantRepository(
            IEncounterContext encounterContext,
            IEncountersClient encountersClient,
            IMapper mapper)
        {
            _encounterContext = encounterContext;
            _encountersClient = encountersClient;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Models.Participant>> GetParticipantsByEncounterId(string encounterId)
        {
            var careTeam = await _encountersClient.ParticipantsAsync(encounterId, _encounterContext.GetXCorrelationId());
            return _mapper.Map<IEnumerable<Models.Participant>>(careTeam);
        }
    }
}
