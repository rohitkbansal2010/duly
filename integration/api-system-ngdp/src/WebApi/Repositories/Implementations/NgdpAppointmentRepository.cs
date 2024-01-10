// <copyright file="NgdpAppointmentRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Common.Infrastructure.Exceptions;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Api.Repositories.Interfaces;
using Duly.Ngdp.Api.Repositories.Mappings.Converters;
using Duly.Ngdp.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IAppointmentRepository"/>
    /// </summary>
    internal class NgdpAppointmentRepository : IAppointmentRepository
    {
        private readonly IAppointmentAdapter _adapter;
        private readonly IMapper _mapper;
        private readonly ITimeZoneConverter _timeZoneConverter;

        public NgdpAppointmentRepository(IAppointmentAdapter adapter, IMapper mapper, ITimeZoneConverter timeZoneConverter)
        {
            _adapter = adapter;
            _mapper = mapper;
            _timeZoneConverter = timeZoneConverter;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsForLocationByDateRangeAsync(
            string departmentId,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            string[] includedVisitTypes,
            AppointmentStatusParam[] excludedAppointmentStatuses = null)
        {
            var searchCriteria = BuildAppointmentSearchCriteria(departmentId, startDate, endDate, includedVisitTypes, excludedAppointmentStatuses);

            var ngdpAppointments = await _adapter.FindAppointmentsAsync(searchCriteria);

            var systemAppointments = _mapper.Map<IEnumerable<Appointment>>(ngdpAppointments);

            return systemAppointments;
        }

        public async Task<Appointment> GetAppointmentByCsnId(string csnId)
        {
            var ngdpAppointment = await _adapter.FindAppointmentByCsnIdAsync(csnId);

            if (ngdpAppointment == null)
            {
                throw new EntityNotFoundException(nameof(Appointment), csnId);
            }

            var systemAppointment = _mapper.Map<Appointment>(ngdpAppointment);

            return systemAppointment;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsForPatientByCsnIdAsync(
            string csnId,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            AppointmentStatusParam[] includedAppointmentStatuses)
        {
            var searchCriteria = BuildAppointmentSearchCriteria(csnId, startDate, endDate, includedAppointmentStatuses);

            var ngdpAppointments = await _adapter.FindAppointmentsForPatientByCsnIdAsync(searchCriteria);

            var systemAppointments = _mapper.Map<IEnumerable<Appointment>>(ngdpAppointments);

            return systemAppointments;
        }

        public async Task<IEnumerable<ReferralAppointment>> GetReferralAppointmentsByReferralIdAsync(string referralId)
        {
            var ngdpAppointments = await _adapter.FindReferralAppointmentsByReferralIdAsync(referralId);

            var systemAppointments = _mapper.Map<IEnumerable<ReferralAppointment>>(ngdpAppointments);

            return systemAppointments;
        }

        private static string[] BuildIncludedVisitTypeIds(string[] includedVisitTypes)
        {
            return includedVisitTypes ?? Array.Empty<string>();
        }

        private AdapterModels.AppointmentSearchCriteria BuildAppointmentSearchCriteria(
            string departmentId,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            string[] includedVisitTypes,
            AppointmentStatusParam[] excludedAppointmentStatuses)
        {
            var searchCriteria = new AdapterModels.AppointmentSearchCriteria
            {
                AppointmentTimeLowerBound = _timeZoneConverter.ToCstDateTime(startDate),
                AppointmentTimeUpperBound = _timeZoneConverter.ToCstDateTime(endDate),
                DepartmentId = departmentId,
                IncludedVisitTypeIds = BuildIncludedVisitTypeIds(includedVisitTypes),
                ExcludedStatuses = BuildExcludedStatuses(excludedAppointmentStatuses)
            };
            return searchCriteria;
        }

        private AdapterModels.AppointmentStatus[] BuildExcludedStatuses(AppointmentStatusParam[] excludedAppointmentStatuses)
        {
            return excludedAppointmentStatuses == null ? Array.Empty<AdapterModels.AppointmentStatus>() : _mapper.Map<AdapterModels.AppointmentStatus[]>(excludedAppointmentStatuses);
        }

        private AdapterModels.AppointmentSearchCriteria BuildAppointmentSearchCriteria(
            string csnId,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            AppointmentStatusParam[] includedAppointmentStatuses)
        {
            var searchCriteria = new AdapterModels.AppointmentSearchCriteria
            {
                CsnId = csnId,
                AppointmentTimeLowerBound = _timeZoneConverter.ToCstDateTime(startDate),
                AppointmentTimeUpperBound = _timeZoneConverter.ToCstDateTime(endDate),
                IncludedStatuses = _mapper.Map<AdapterModels.AppointmentStatus[]>(includedAppointmentStatuses)
            };
            return searchCriteria;
        }
    }
}