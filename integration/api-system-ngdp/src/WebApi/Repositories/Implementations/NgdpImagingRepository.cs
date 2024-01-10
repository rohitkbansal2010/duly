// <copyright file="NgdpImagingRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Api.Repositories.Interfaces;
using Duly.Ngdp.Contracts;
using System.Threading.Tasks;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Implementations
{
    public class NgdpImagingRepository : IImagingRepository
    {
        private readonly IImagingAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpImagingRepository(IImagingAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<int> PostImagingAsync(ImagingDetail request)
        {
            var result = _mapper.Map<AdapterModels.ImagingDetail>(request);
            var ngdpImaging = _adapter.ImagingAsync(result);
            return await ngdpImaging;
        }
    }
}