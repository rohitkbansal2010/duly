// <copyright file="NgdpConditionRepository.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="IConditionRepository"/>
    /// </summary>
    public class NgdpConditionRepository : IConditionRepository
    {
        private readonly IConditionAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpConditionRepository(IConditionAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Condition>> GetConditionsAsync()
        {
            var ngdpConditionData = await _adapter.GetConditionsAsync();
            var result = _mapper.Map<List<Condition>>(ngdpConditionData);
            return result;
        }
    }
}