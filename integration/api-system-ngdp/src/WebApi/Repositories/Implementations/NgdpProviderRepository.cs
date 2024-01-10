// <copyright file="NgdpProviderRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Api.Repositories.Interfaces;
using Duly.Ngdp.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IProviderRepository"/>
    /// </summary>
    public class NgdpProviderRepository : IProviderRepository
    {
        private readonly IProviderAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpProviderRepository(IProviderAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProviderLocation>> GetProvidersByLatLng(string lat, string lng, string providerType)
        {
            var searchCriteria = new AdapterModels.ProviderSearchCriteria
            {
                Lat = lat,
                Lng = lng,
                ProviderType = providerType
            };

            var ngdpProviders = await _adapter.FindProviderByLatLngAsync(searchCriteria);

            var systemProviders = _mapper.Map<IEnumerable<ProviderLocation>>(ngdpProviders);

            return systemProviders;
        }

        public async Task<IEnumerable<ProviderDetails>> GetProviderDetailsAsync(string providerIds)
        {
            var ngdpGetProviderDetails = await _adapter.FindProviderDetailsAsync(providerIds);
            var result = _mapper.Map<IEnumerable<ProviderDetails>>(ngdpGetProviderDetails);
            return result;
        }
    }
}
