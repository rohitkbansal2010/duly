// <copyright file="NgdpCustomTargetsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan;
using Duly.Ngdp.Api.Repositories.Interfaces.CarePlan;
using Duly.Ngdp.Contracts.CarePlan;
using System.Threading.Tasks;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models.CarePlan;

namespace Duly.Ngdp.Api.Repositories.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="ICustomTargetsRepository"/>
    /// </summary>
    public class NgdpCustomTargetsRepository : ICustomTargetsRepository
    {
        private readonly ICustomTargetsAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpCustomTargetsRepository(ICustomTargetsAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<int> PostCustomTargetsAsync(CustomTargets request)
        {
            var result = _mapper.Map<AdapterModels.CustomTargets>(request);
            var ngdpCustomTargets = _adapter.PostCustomTargetsAsync(result);
            return await ngdpCustomTargets;
        }
    }
}
