// <copyright file="NgdpConditionTargetsRepository.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="IConditionTargetsRepository"/>
    /// </summary>
    public class NgdpConditionTargetsRepository : IConditionTargetsRepository
    {
        private readonly IConditionTargetsAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpConditionTargetsRepository(IConditionTargetsAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ConditionTargets>> GetConditionTargetsByConditionIdAsync(string conditionIds)
        {
            var ngdpConditionTargetsData = await _adapter.GetConditionTargetsByConditionIdAsync(conditionIds);
            var result = _mapper.Map<List<ConditionTargets>>(ngdpConditionTargetsData);
            return result;
        }
    }
}