// <copyright file="NgdpPatientTargetsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan;
using Duly.Ngdp.Api.Repositories.Interfaces.CarePlan;
using Duly.Ngdp.Contracts.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="IConditionRepository"/>
    /// </summary>
    public class NgdpPatientTargetsRepository : IPatientTargetsRepository
    {
        private readonly IPatientTargetsAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpPatientTargetsRepository(IPatientTargetsAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetPatientTargets>> GetPatientTargetsAsync(long patientPlanId)
        {
            var ngdpPatientTargetsData = await _adapter.GetPatientTargetsAsync(patientPlanId);
            var result = _mapper.Map<List<GetPatientTargets>>(ngdpPatientTargetsData);
            return result;
        }

        public async Task<int> PostPatientTargetsAsync(PatientTarget request)
        {
            var mappedRequest = _mapper.Map<AdapterModels.CarePlan.PatientTarget>(request);
            var result = await _adapter.PostPatientTargetAsync(mappedRequest);
            return result;
        }

        public async Task<int> DeletePatientTargetAsync(long id)
        {
            var result = await _adapter.DeletePatientTargetAsync(id);
            return result;
        }
    }
}