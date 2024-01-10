// -----------------------------------------------------------------------
// <copyright file="GetSlotDataRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using AutoMapper;

using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.Ngdp.Api.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IGetSlotDataRepository"/>
    /// </summary>
    internal class GetSlotDataRepository : IGetSlotDataRepository
    {
        private readonly IEncounterContext _encounterContext;
        private readonly IAppointmentIdClient _appointmentIdClient;
        private readonly IMapper _mapper;

        public GetSlotDataRepository(
            IEncounterContext encounterContext,
            IMapper mapper,
            IAppointmentIdClient appointmentIdClient)
        {
            _encounterContext = encounterContext;
            _mapper = mapper;
            _appointmentIdClient = appointmentIdClient;
        }

        public async Task<Models.AppointmentByCsnId> GetProvidersByAppointmentIdAsync(string appointmentId)
        {
            var ngdpProviders = await _appointmentIdClient.SlotDataAsync(appointmentId, _encounterContext.GetXCorrelationId());

            return _mapper.Map<Models.AppointmentByCsnId>(ngdpProviders);
        }
    }
}
