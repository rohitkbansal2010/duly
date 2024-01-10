// -----------------------------------------------------------------------
// <copyright file="PractitionerRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using AutoMapper;
using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IPractitionerRepository"/>
    /// </summary>
    internal class PractitionerRepository : IPractitionerRepository
    {
        private readonly IEncounterContext _encounterContext;
        private readonly IMapper _mapper;
        private readonly ISitesClient _sitesClient;
        private readonly IClient _client;

        public PractitionerRepository(
            IEncounterContext encounterContext,
            IMapper mapper,
            ISitesClient sitesClient,
            IClient client)
        {
            _encounterContext = encounterContext;
            _mapper = mapper;
            _sitesClient = sitesClient;
            _client = client;
        }

        public async Task<IEnumerable<Models.PractitionerGeneralInfo>> GetPractitionersBySiteIdAsync(string siteId)
        {
            var practitioners = await _sitesClient.PractitionersAsync(siteId, _encounterContext.GetXCorrelationId());
            return _mapper.Map<IEnumerable<Models.PractitionerGeneralInfo>>(practitioners);
        }

        public async Task<IEnumerable<Models.PractitionerGeneralInfo>> GetPractitionersByIdsAsync(string[] practitionerIds)
        {
            if (practitionerIds.Length == 0)
                return Array.Empty<Models.PractitionerGeneralInfo>();

            var practitioners = await _client.PractitionersAsync(practitionerIds, _encounterContext.GetXCorrelationId());
            return _mapper.Map<IEnumerable<Models.PractitionerGeneralInfo>>(practitioners);
        }
    }
}
