// <copyright file="NgdpPatientRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Api.Repositories.Interfaces;
using Duly.Ngdp.Contracts;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Implementations
{
    public class NgdpPatientRepository : IPatientRepository
    {
        private readonly IRecommendedProviderAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpPatientRepository(IRecommendedProviderAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<ReferralPatient> GetPatientByReferralIdAsync(string referralId)
        {
            var recommendedProvider = await _adapter.FindFirstRecommendedProviderByReferralIdAsync(referralId);
            return _mapper.Map<ReferralPatient>(recommendedProvider);
        }
    }
}