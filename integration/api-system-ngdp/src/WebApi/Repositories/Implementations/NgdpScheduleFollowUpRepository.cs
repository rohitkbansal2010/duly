// <copyright file="NgdpScheduleFollowUpRepository.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="IScheduleFollowUpRepository"/>
    /// </summary>
    internal class NgdpScheduleFollowUpRepository : IScheduleFollowUpRepository
    {
        private readonly IScheduleFollowUpAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpScheduleFollowUpRepository(IScheduleFollowUpAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<int> PostScheduleFollowUpAsync(ScheduleFollowUp request)
        {
            var result = _mapper.Map<AdapterModels.ScheduleFollowUp>(request);
            var data = _adapter.PostScheduleFollowUpAsync(result);
            return await data;
        }
    }
}