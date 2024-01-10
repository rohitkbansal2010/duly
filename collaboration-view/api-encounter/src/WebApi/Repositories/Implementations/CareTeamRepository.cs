// -----------------------------------------------------------------------
// <copyright file="CareTeamRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using AutoMapper;
using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

using Model = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    internal class CareTeamRepository : ICareTeamRepository
    {
        private readonly IEncounterContext _encounterContext;
        private readonly ICareTeamClient _careTeamClient;
        private readonly IMapper _mapper;

        public CareTeamRepository(
            IEncounterContext encounterContext,
            ICareTeamClient careTeamClient,
            IMapper mapper)
        {
            _encounterContext = encounterContext;
            _careTeamClient = careTeamClient;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Models.CareTeamParticipant>> GetCareTeamParticipantsByPatientIdAsync(string patientId, Model.CareTeamStatus status, Model.CareTeamCategory category)
        {
            var careTeamStatus = _mapper.Map<CareTeamStatus>(status);
            var careTeamCategory = _mapper.Map<CareTeamCategory>(category);

            var careTeam = await _careTeamClient.MembersAsync(
                patientId,
                careTeamStatus,
                careTeamCategory,
                _encounterContext.GetXCorrelationId());

            return _mapper.Map<IEnumerable<Models.CareTeamParticipant>>(careTeam);
        }
    }
}
