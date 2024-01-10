// <copyright file="NgdpRecommendedProviderRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Api.Repositories.Interfaces;
using Duly.Ngdp.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Implementations
{
    public class NgdpRecommendedProviderRepository : IRecommendedProviderRepository
    {
        private readonly IRecommendedProviderAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpRecommendedProviderRepository(IRecommendedProviderAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RecommendedProvider>> GetRecommendedProvidersByReferralIdAsync(string referralId)
        {
            var recommendedProviders = await _adapter.FindRecommendedProvidersByReferralIdAsync(referralId);
            return _mapper.Map<IEnumerable<RecommendedProvider>>(recommendedProviders);
        }
    }
}
