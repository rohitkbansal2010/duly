// -----------------------------------------------------------------------
// <copyright file="ServiceRequestRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using AutoMapper;
using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IServiceRequestRepository"/>
    /// </summary>
    internal class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly IEncounterContext _encounterContext;
        private readonly IAppointmentIdClient _client;
        private readonly IMapper _mapper;

        public ServiceRequestRepository(
            IEncounterContext encounterContext,
            IMapper mapper,
            IAppointmentIdClient client)
        {
            _encounterContext = encounterContext;
            _mapper = mapper;
            _client = client;
        }

        public async Task<Models.ServiceRequest> GetLabsOrImagingAsync(string patientId, string appointmentId, string type)
        {
            //Change the client method to called.
            var LabsOrImaging = await _client.TypeAsync(patientId, appointmentId, type, _encounterContext.GetXCorrelationId());
            var res = new Models.ServiceRequest();
            res.TestOrder = _mapper.Map<List<Models.Orders>>(LabsOrImaging.TestOrder);
            res.OrderCount = LabsOrImaging.OrderCount;
            return res;
        }
    }
}
