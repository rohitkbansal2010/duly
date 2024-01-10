// <copyright file="NgdpPatientLifeGoalsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan;
using Duly.Ngdp.Api.Repositories.Interfaces.CarePlan;
using Duly.Ngdp.Contracts.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models.CarePlan;

namespace Duly.Ngdp.Api.Repositories.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="IPatientLifeGoalsRepository"/>
    /// </summary>
    public class NgdpPatientLifeGoalsRepository : IPatientLifeGoalsRepository
    {
        private readonly IPatientLifeGoalsAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpPatientLifeGoalsRepository(IPatientLifeGoalsAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetPatientLifeGoals>> GetPatientLifeGoalsAsync(long patientPlanId)
        {
            var ngdpPatientLifeGoalsData = await _adapter.GetPatientLifeGoalsAsync(patientPlanId);
            var result = _mapper.Map<List<GetPatientLifeGoals>>(ngdpPatientLifeGoalsData);
            return result;
        }

        public async Task<List<PatientLifeGoalResponse>> PostOrUpdateLifeGoalAsync(IEnumerable<PatientLifeGoals> request)
        {
            var result = _mapper.Map<IEnumerable<AdapterModels.PatientLifeGoals>>(request);
            var ngdpLifeGoal = await _adapter.PostOrUpdateLifeGoalAsync(result);

            var response = _mapper.Map<List<PatientLifeGoalResponse>>(ngdpLifeGoal);
            return response;
        }

        public async Task<long> DeletePatientLifeGoalAsync(long id)
        {
            var result = await _adapter.DeletePatientLifeGoalAsync(id);
            return result;
        }
    }
}
