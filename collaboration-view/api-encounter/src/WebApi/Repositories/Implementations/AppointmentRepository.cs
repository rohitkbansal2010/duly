// -----------------------------------------------------------------------
// <copyright file="AppointmentRepository.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="IAppointmentRepository"/>
    /// </summary>
    internal class AppointmentRepository : IAppointmentRepository
    {
        private readonly IEncounterContext _encounterContext;
        private readonly ILocationsClient _locationsClient;
        private readonly IAppointmentsClient _appointmentsClient;
        private readonly IClient _client;
        private readonly IMapper _mapper;

        public AppointmentRepository(
            IEncounterContext encounterContext,
            ILocationsClient locationsClient,
            IAppointmentsClient appointmentsClient,
            IClient client,
            IMapper mapper)
        {
            _encounterContext = encounterContext;
            _locationsClient = locationsClient;
            _appointmentsClient = appointmentsClient;
            _client = client;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Models.Appointment>> GetAppointmentsBySiteIdAndDateRangeAsync(
            string siteId,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            string[] includedVisitTypes,
            Models.AppointmentStatusParam[] excludedStatuses = null)
        {
            var ngdpExcludedStatuses = _mapper.Map<IEnumerable<AppointmentStatusParam>>(excludedStatuses);

            var ngdpAppointments = await _locationsClient
                .AppointmentsAsync(siteId, startDate, endDate, includedVisitTypes, _encounterContext.GetXCorrelationId(), ngdpExcludedStatuses);

            var repositoryAppointments = _mapper.Map<IEnumerable<Models.Appointment>>(ngdpAppointments);

            return repositoryAppointments;
        }

        public async Task<IEnumerable<Models.Appointment>> GetAppointmentsForSamePatientByAppointmentIdAsync(
            string appointmentId,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            Models.AppointmentStatusParam[] includedStatuses)
        {
            var ngdpIncludedStatuses = _mapper.Map<IEnumerable<AppointmentStatusParam>>(includedStatuses);

            var ngdpAppointments = await _appointmentsClient
                .ForSamePatientAsync(
                    appointmentId,
                    startDate,
                    endDate,
                    ngdpIncludedStatuses,
                    _encounterContext.GetXCorrelationId());

            var repositoryAppointments = _mapper.Map<IEnumerable<Models.Appointment>>(ngdpAppointments);

            return repositoryAppointments;
        }

        public async Task<Models.Appointment> GetAppointmentByIdAsync(string appointmentId)
        {
            var ngdpAppointment = await _client.AppointmentsAsync(appointmentId, _encounterContext.GetXCorrelationId());

            var repositoryAppointment = _mapper.Map<Models.Appointment>(ngdpAppointment);
            return repositoryAppointment;
        }
    }
}
