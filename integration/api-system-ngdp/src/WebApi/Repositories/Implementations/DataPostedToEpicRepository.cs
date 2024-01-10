// <copyright file="DataPostedToEpicRepository.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="IDataPostedToEpicRepository"/>
    /// </summary>
    public class DataPostedToEpicRepository : IDataPostedToEpicRepository
    {
        private readonly IDataPostedToEpicAdapter _adapter;
        private readonly IMapper _mapper;

        public DataPostedToEpicRepository(IDataPostedToEpicAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<int> DataPostedToEpicAsync(int id)
        {
            var ngdpDataPosted = _adapter.PostDataPostedToEpicAsync(id);
            return await ngdpDataPosted;
        }
    }
}