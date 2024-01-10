// <copyright file="NgdpSlotDataRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Api.Repositories.Interfaces;
using Duly.Ngdp.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="ISlotDataRepository"/>
    /// </summary>
    internal class NgdpSlotDataRepository : ISlotDataRepository
    {
        private readonly ISlotDataAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpSlotDataRepository(ISlotDataAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<DepartmentVisitType> GetSlotDataAsync(string appointmentId)
        {
            var ngdpSlotData = await _adapter.FindSlotDataAsync(appointmentId);
            var result = _mapper.Map<DepartmentVisitType>(ngdpSlotData);
            return result;
        }
    }
}