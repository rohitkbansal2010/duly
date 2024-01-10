// <copyright file="AppointmentRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Contracts;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Wipfli.Adapter.Adapters.Interfaces;
using Wipfli.Adapter.Client;
using Wipfli.Adapter.Configuration;

namespace Duly.Clinic.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IAppointmentRepository"/>
    /// </summary>
    internal class AppointmentRepository : IAppointmentRepository
    {
        private readonly IOptionsMonitor<PrivateApiOptions> _optionsMonitor;
        private readonly IScheduleAdapter _scheduleAdapter;
        private readonly IMapper _mapper;

        public AppointmentRepository(
            IOptionsMonitor<PrivateApiOptions> optionsMonitor,
            IScheduleAdapter scheduleAdapter,
            IMapper mapper)
        {
            _optionsMonitor = optionsMonitor;
            _scheduleAdapter = scheduleAdapter;
            _mapper = mapper;
        }

        public async Task<ScheduledAppointment> ScheduleAppointmentAsync(ScheduleAppointmentModel model)
        {
            var patientId = model.PatientId.SplitIdentifier();
            var providerId = model.ProviderId.SplitIdentifier();
            var departmentId = model.DepartmentId.SplitIdentifier();
            var typeId = model.VisitTypeId.SplitIdentifier();

            var adapterRequest = new ScheduleAppointmentWithInsuranceRequest
            {
                Date = model.Date.Value,
                Time = model.Time.Value,
                PatientID = patientId.ID,
                PatientIDType = patientId.Type,
                ProviderID = providerId.ID,
                ProviderIDType = providerId.Type,
                DepartmentID = departmentId.ID,
                DepartmentIDType = departmentId.Type,
                VisitTypeID = typeId.ID,
                VisitTypeIDType = typeId.Type,
                IsReviewOnly = _optionsMonitor.CurrentValue.BypassAppointmentCreation
            };

            var appointment = await _scheduleAdapter.ScheduleAppointment(adapterRequest);
            var systemAppointment = _mapper.Map<ScheduledAppointment>(appointment.Appointment);

            return systemAppointment;
        }
    }
}
