// <copyright file="NgdpPatientConditionsRepository.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="IPatientConditionsRepository"/>
    /// </summary>
    public class NgdpPatientConditionsRepository : IPatientConditionsRepository
    {
        private readonly IPatientConditionsAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpPatientConditionsRepository(IPatientConditionsAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetPatientConditions>> GetPatientConditionsAsync(long patientPlanId)
        {
            var ngdpPatientConditionsData = await _adapter.GetPatientConditionsAsync(patientPlanId);
            var result = _mapper.Map<List<GetPatientConditions>>(ngdpPatientConditionsData);
            return result;
        }

        public async Task<long> PostPatientConditionsAsync(PatientConditions request)
        {
            var result = _mapper.Map<AdapterModels.PatientConditions>(request);
            var ngdpPatientConditions = _adapter.PostPatientConditionsAsync(result);
            return await ngdpPatientConditions;
        }
    }
}
