// -----------------------------------------------------------------------
// <copyright file="LabDetailRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// --------------

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using Duly.Ngdp.Api.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="ILabDetailRepository"/>
    /// </summary>
    internal class LabDetailRepository : ILabDetailRepository
    {
        private readonly IEncounterContext _encounterContext;
        private readonly IMapper _mapper;
        private readonly IClient _checkOutDetailsClient;
        private readonly ILatlngClient _latlngClient;

        public LabDetailRepository(
                 IEncounterContext encounterContext,
                 IMapper mapper,
                 IClient checkOutDetailsClient,
                 ILatlngClient latlngClient)
        {
            _encounterContext = encounterContext;
            _mapper = mapper;
            _checkOutDetailsClient = checkOutDetailsClient;
            _latlngClient = latlngClient;
        }

        public async Task<int> PostLabDetailAsync(Models.CheckOut.LabDetails request)
        {
            var ngdpLabDetails = _mapper.Map<Ngdp.Api.Client.LabDetails>(request);
            var responseNgdpLabDetail = await _checkOutDetailsClient.LabDetailsAsync(_encounterContext.GetXCorrelationId(), ngdpLabDetails);
            return responseNgdpLabDetail;
        }

        public async Task<IEnumerable<Models.LabLocation>> GetLabLocationByLatLngAsync(string lat, string lng)
        {
            var ngdpAppointments = await _latlngClient.LabDetailsAsync(lat, lng, _encounterContext.GetXCorrelationId());

            var repositoryAppointments = _mapper.Map<IEnumerable<Models.LabLocation>>(ngdpAppointments);
            return repositoryAppointments;
        }
    }
}
