// <copyright file="AfterVisitPdfService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IAfterVisitPdfService"/>
    /// </summary>
    internal class AfterVisitPdfService : IAfterVisitPdfService
    {
        private readonly IMapper _mapper;
        private readonly IAfterVisitPdfRepository _repository;
        public AfterVisitPdfService(IMapper mapper, IAfterVisitPdfRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<int> PostAfterVisitPdfAsync(AfterVisitPdf request)
        {
            var requestAfterVisitPdf = _mapper.Map<Models.AfterVisitPdf>(request);
            var responseAfterVisitPdf = await _repository.PostAfterVisitPdfAsync(requestAfterVisitPdf);
            return responseAfterVisitPdf;
        }

        public async Task<string> GetAfterVisitPdfById(long aftervisitpdfid)
        {
            var responseAfterVisitPdfId = await _repository.GetAfterVisitPdfAsync(aftervisitpdfid);
            return responseAfterVisitPdfId;
        }

        public async Task<long> UpdateAfterVisitPdfIsSMSSentAsync(long id, bool isSMSSent)
        {
            var responseAfterVisitPdf = await _repository.UpdateAfterVisitPdfIsSMSSentAsync(id, isSMSSent);
            return responseAfterVisitPdf;
        }
    }
}