// <copyright file="NgdpLabDetailsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Api.Repositories.Interfaces;
using Duly.Ngdp.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="ILabDetailsRepository"/>
    /// </summary>
    public class NgdpLabDetailsRepository : ILabDetailsRepository
    {
        private readonly ILabDetailsAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpLabDetailsRepository(ILabDetailsAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<int> PostLabDetailsAsync(LabDetails request)
        {
            var result = _mapper.Map<AdapterModels.LabDetails>(request);
            var ngdpLabDetails = _adapter.PostLabDetailsAsync(result);
            return await ngdpLabDetails;
        }

        public async Task<IEnumerable<LabLocation>> GetLabLocationByLatLng(string lat, string lng)
        {
            var searchCriteria = new AdapterModels.LabLocationSearchCriteria
            {
                Lat = lat,
                Lng = lng
            };

            var ngdplabs = await _adapter.FindLabLocationByLatLngAsync(searchCriteria);

            var systemlabs = _mapper.Map<IEnumerable<LabLocation>>(ngdplabs);

            return systemlabs;
        }
    }
}