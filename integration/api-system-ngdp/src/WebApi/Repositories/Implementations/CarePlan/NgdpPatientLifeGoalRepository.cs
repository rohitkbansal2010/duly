// <copyright file="NgdpPatientLifeGoalRepository.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="IPatientLifeGoalRepository"/>
    /// </summary>
    public class NgdpPatientLifeGoalRepository : IPatientLifeGoalRepository
    {
        private readonly IPatientLifeGoalAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpPatientLifeGoalRepository(IPatientLifeGoalAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<int> PostPatientLifeGoalAsync(PatientLifeGoal request)
        {
            var result = _mapper.Map<AdapterModels.PatientLifeGoal>(request);
            var ngdpLifeGoal = _adapter.PostPatientLifeGoalAsync(result);
            return await ngdpLifeGoal;
        }
    }
}
