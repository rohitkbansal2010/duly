// <copyright file="ServiceRequestService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IServiceRequestService"/>
    /// </summary>
    internal class ServiceRequestService : IServiceRequestService
    {
        private readonly IMapper _mapper;
        private readonly IServiceRequestRepository _repository;

        public ServiceRequestService(
            IMapper mapper,
            IServiceRequestRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceRequest> GetLabOrImagingOrdersAsync(string patientId, string appointmentId, string type)
        {
            var LabOrImaging = await _repository.GetLabsOrImagingAsync(patientId, appointmentId, type);
            var res = new ServiceRequest();
            res.TestOrder = _mapper.Map<List<Orders>>(LabOrImaging.TestOrder);
            res.OrderCount = LabOrImaging.OrderCount;
            return res;
        }
    }
}
