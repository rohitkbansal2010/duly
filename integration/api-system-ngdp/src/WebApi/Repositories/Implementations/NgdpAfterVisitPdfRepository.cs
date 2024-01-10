// <copyright file="NgdpAfterVisitPdfRepository.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="IAfterVisitPdfRepository"/>
    /// </summary>
    internal class NgdpAfterVisitPdfRepository : IAfterVisitPdfRepository
    {
        private readonly IAfterVisitPdfAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpAfterVisitPdfRepository(IAfterVisitPdfAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<int> PostAfterVisitPdfAsync(AfterVisitPdf request)
        {
            var result = _mapper.Map<AdapterModels.AfterVisitPdf>(request);
            var ngdpAfterVisitPdf = _adapter.AfterVisitPdfAsync(result);
            return await ngdpAfterVisitPdf;
        }

        public async Task<string> GetAfterVisitPdfByAfterVisitPdfIdAsync(long afterVisitPdfId)
        {
            var ngdpAfterVisitPdfData = await _adapter.GetAfterVisitPdfByAfrerVisitPdfIdAsync(afterVisitPdfId);
            return ngdpAfterVisitPdfData;
        }
    }
}
