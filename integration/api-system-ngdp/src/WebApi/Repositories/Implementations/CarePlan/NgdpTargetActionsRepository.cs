// <copyright file="NgdpTargetActionsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan;
using Duly.Ngdp.Api.Repositories.Interfaces.CarePlan;
using Duly.Ngdp.Contracts.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="ITargetActionsRepository"/>
    /// </summary>
    public class NgdpTargetActionsRepository : ITargetActionsRepository
    {
        private readonly ITargetActionsAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpTargetActionsRepository(ITargetActionsAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TargetActions>> GetTargetActionsByTargetIdAsync(long targetId)
        {
            var ngdpTargetActionsData = await _adapter.GetTargetActionsByTargetIdAsync(targetId);
            var result = _mapper.Map<List<TargetActions>>(ngdpTargetActionsData);
            return result;
        }
    }
}