// <copyright file="NgdpCustomActionsRepository.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="ICustomActionssRepository"/>
    /// </summary>
    public class NgdpCustomActionsRepository : ICustomActionsRepository
    {
        private readonly ICustomActionsAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpCustomActionsRepository(ICustomActionsAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<int> PostCustomActionsAsync(CustomActions request)
        {
            var result = _mapper.Map<AdapterModels.CustomActions>(request);
            var ngdpCustomActions = _adapter.PostCustomTargetsAsync(result);
            return await ngdpCustomActions;
        }
    }
}
