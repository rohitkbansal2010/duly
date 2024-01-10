// -----------------------------------------------------------------------
// <copyright file="ScheduleSlotsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using AutoMapper;
using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IGetSlotsRepository"/>
    /// </summary>
    internal class ScheduleSlotsRepository : IScheduleSlotsRepository
    {
        private readonly IEncounterContext _encounterContext;
        private readonly IClient _client;
        private readonly IMapper _mapper;

        public ScheduleSlotsRepository(
            IEncounterContext encounterContext,
            IMapper mapper,
            IClient client)
        {
            _encounterContext = encounterContext;
            _mapper = mapper;
            _client = client;
        }

        public async Task<Models.CheckOut.ScheduledAppointment> ScheduleAppointmentForPatient(Models.CheckOut.ScheduleAppointmentModel model)
        {
            var body = _mapper.Map<ScheduleAppointmentModel>(model);
            var scheduledAppointment = await _client.AppointmentsAsync(_encounterContext.GetXCorrelationId(), body);
            return _mapper.Map<Models.CheckOut.ScheduledAppointment>(scheduledAppointment);
        }
    }
}
