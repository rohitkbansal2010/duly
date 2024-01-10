// <copyright file="NgdpPatientPlanRepository.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="IPatientPlanRepository"/>
    /// </summary>
    public class NgdpPatientPlanRepository : IPatientPlanRepository
    {
        private readonly IPatientPlanAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpPatientPlanRepository(IPatientPlanAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetPatientPlan>> GetPatientPlanAsync(string patientId)
        {
            var ngdpPatientPlanData = await _adapter.GetPatientPlanAsync(patientId);
            var result = _mapper.Map<List<GetPatientPlan>>(ngdpPatientPlanData);
            return result;
        }

        public async Task<int> PostPatientPlanAsync(PatientPlan request)
        {
            var result = _mapper.Map<AdapterModels.PatientPlan>(request);
            var ngdpPatientPlan = _adapter.PostPatientPlanAsync(result);
            return await ngdpPatientPlan;
        }

        public async Task<bool> UpdatePatientPlanStatusByIdAsync(long id)
        {
            var ngdpPatientPlanStatus = _adapter.UpdatePatientPlanStatusByIdAsync(id);
            return await ngdpPatientPlanStatus;
        }
    }
}
