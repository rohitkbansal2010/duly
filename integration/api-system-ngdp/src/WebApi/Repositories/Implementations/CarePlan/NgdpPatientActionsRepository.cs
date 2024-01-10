// <copyright file="NgdpPatientActionsRepository.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="ICustomActionssRepository"/>
    /// </summary>
    public class NgdpPatientActionsRepository : IPatientActionsRepository
    {
        private readonly IPatientActionsAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpPatientActionsRepository(IPatientActionsAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetPatientActions>> GetPatientActionsAsync(long patientTargetId)
        {
            var ngdpPatientActionsData = await _adapter.GetPatientActionsAsync(patientTargetId);
            var result = _mapper.Map<List<GetPatientActions>>(ngdpPatientActionsData);
            return result;
        }

        public async Task<long> PostPatientActionsAsync(IEnumerable<PatientActions> request)
        {
            var result = _mapper.Map<IEnumerable<AdapterModels.PatientActions>>(request);
            var ngdpPatientActions = _adapter.PostPatientActionsAsync(result);
            return await ngdpPatientActions;
        }
    }
}
