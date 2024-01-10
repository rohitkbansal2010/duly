// <copyright file="NgdpReferralDetailRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Common.Infrastructure.Exceptions;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Api.Repositories.Interfaces;
using Duly.Ngdp.Api.Repositories.Mappings.Converters;
using Duly.Ngdp.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IReferralDetailRepository"/>
    /// </summary>
    internal class NgdpReferralDetailRepository : IReferralDetailRepository
    {
        private readonly IReferralDetailAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpReferralDetailRepository(IReferralDetailAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<int> PostReferralDetailAsync(ReferralDetail request)
        {
            var result = _mapper.Map<AdapterModels.ReferralDetail>(request);

            var data = _adapter.PostReferralDetailAsync(result);
            return await data;
        }
    }
}