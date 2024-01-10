// <copyright file="ImagingDetailService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IImagingDetailService"/>
    /// </summary>
    internal class ImagingDetailService : IImagingDetailService
    {
        private readonly IMapper _mapper;
        private readonly IImagingDetailRepository _repository;
        public ImagingDetailService(IMapper mapper, IImagingDetailRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<int> PostImagingDetailAsync(ImagingDetail request)
        {
            if (string.IsNullOrEmpty(request.ImagingType))
                request.ImagingType = "Imaging";

            var requestImagingDetails = _mapper.Map<Models.ImagingDetails>(request);
            var responseImagingDetails = await _repository.PostImagingDetailAsync(requestImagingDetails);
            return responseImagingDetails;
        }
    }
}