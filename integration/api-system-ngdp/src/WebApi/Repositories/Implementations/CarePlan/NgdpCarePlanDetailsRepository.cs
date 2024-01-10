// <copyright file="NgdpCarePlanDetailsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan;
using Duly.Ngdp.Api.Repositories.Interfaces.CarePlan;
using Duly.Ngdp.Contracts.CarePlan;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="ICarePlanDetailsRepository"/>
    /// </summary>
    public class NgdpCarePlanDetailsRepository : ICarePlanDetailsRepository
    {
        private readonly ICarePlanDetailsAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpCarePlanDetailsRepository(ICarePlanDetailsAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CarePlanDetails>> GetCarePlanDetailsAsync(string patientId)
        {
            var ngdpCarePlanData = await _adapter.GetCarePlanDetailsAsync(patientId);
            var result = _mapper.Map<List<CarePlanDetails>>(ngdpCarePlanData);
            return result;
        }
    }
}