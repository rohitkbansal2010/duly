// -----------------------------------------------------------------------
// <copyright file="ProviderRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.Ngdp.Api.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IProviderRepository"/>
    /// </summary>
    public class ProviderRepository : IProviderRepository
    {
        private readonly IEncounterContext _encounterContext;
        private readonly IClient _clientProvider;
        private readonly ILatlngClient _client;
        private readonly IMapper _mapper;

        public ProviderRepository(
            IEncounterContext encounterContext,
            IClient clientProvider,
            ILatlngClient client,
            IMapper mapper)
        {
            _encounterContext = encounterContext;
            _client = client;
            _mapper = mapper;
            _clientProvider = clientProvider;
        }

        public async Task<IEnumerable<Models.Provider>> GetProvidersByLatLngAsync(string lat, string lng, string providerType)
        {
            var ngdpAppointments = await _client.ProviderLocationAsync(lat, lng, providerType, _encounterContext.GetXCorrelationId());

            var repositoryAppointments = _mapper.Map<IEnumerable<Models.Provider>>(ngdpAppointments);

            return repositoryAppointments;
        }

        public async Task<IEnumerable<Models.CheckOut.ProviderDetails>> GetProviderDetailsAsync(string providerIds)
        {
            var providerDetails = await _clientProvider.ProvidersAsync(providerIds, _encounterContext.GetXCorrelationId());
            var result = _mapper.Map<IEnumerable<Models.CheckOut.ProviderDetails>>(providerDetails);
            return result;
        }
    }
}
