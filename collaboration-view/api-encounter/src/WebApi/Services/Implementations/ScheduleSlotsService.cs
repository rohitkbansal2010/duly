// <copyright file="ScheduleSlotsService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Services.Extensions;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IScheduleSlotsService"/>
    /// </summary>
    internal class ScheduleSlotsService : IScheduleSlotsservice
    {
        private readonly IMapper _mapper;
        private readonly IScheduleSlotsRepository _repository;
        private readonly IGetSlotDataRepository _getSlotDataRepository;
        private readonly IPatientRepository _patientRepository;

        public ScheduleSlotsService(
            IMapper mapper,
            IScheduleSlotsRepository repository,
            IGetSlotDataRepository getSlotDataRepository,
            IPatientRepository patientRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _getSlotDataRepository = getSlotDataRepository;
            _patientRepository = patientRepository;
        }

        public async Task<ScheduleAppointmentResult> ScheduleAppointmentForPatientAsync(AppointmentSchedulingModel appointmentSchedulingRequest, string appointmentId)
        {
            var reqData = await _getSlotDataRepository.GetProvidersByAppointmentIdAsync(appointmentId);

            var filterVisitTypeIdId = "External|" + appointmentSchedulingRequest.VisitTypeId;

            var model = new Models.CheckOut.ScheduleAppointmentModel
            {
                Date = (System.DateTimeOffset)appointmentSchedulingRequest.Date,
                Time = (System.TimeSpan)appointmentSchedulingRequest.Time,
                PatientId = reqData.PatientId,
                ProviderId = reqData.ProviderId,
                DepartmentId = reqData.DepartmentId,
                VisitTypeId = filterVisitTypeIdId
            };

            var scheduledAppointment = await _repository.ScheduleAppointmentForPatient(model);

            var result = _mapper.Map<ScheduleAppointmentResult>(scheduledAppointment);
            return result;
        }

        public async Task<ScheduleAppointmentResult> ScheduleImagingAppointmentForPatientAsync(Models.CheckOut.ScheduleAppointmentModel model)
        {
            var patient = await _patientRepository.GetPatientByIdAsync(model.PatientId);
            var filterPatientId = patient.Identifiers.ToList().Find(s => s.StartsWith("EXTERNAL|")).Split("|")[1];
            model.PatientId = "External|" + filterPatientId;
            var scheduledAppointment = await _repository.ScheduleAppointmentForPatient(model);

            var result = _mapper.Map<ScheduleAppointmentResult>(scheduledAppointment);
            return result;
        }

        public async Task<ScheduleAppointmentResult> ScheduleReferralAppointmentForPatientAsync(
            AppointmentSchedulingModel appointmentSchedulingRequest,
            string patientId,
            string providerId,
            string departmentId,
            string visitTypeId)
        {
            var filterproviderId = "External|" + providerId;
            var filterPatientId = "External|" + patientId;
            var filterDepartmentId = "External|" + departmentId;
            var fileterVisitTypeId = "External|" + visitTypeId;

            var model = new Models.CheckOut.ScheduleAppointmentModel
            {
                Date = (System.DateTimeOffset)appointmentSchedulingRequest.Date,
                Time = (System.TimeSpan)appointmentSchedulingRequest.Time,
                PatientId = filterPatientId,
                ProviderId = filterproviderId,
                DepartmentId = filterDepartmentId,
                VisitTypeId = fileterVisitTypeId
            };

            var scheduledAppointment = await _repository.ScheduleAppointmentForPatient(model);

            var result = _mapper.Map<ScheduleAppointmentResult>(scheduledAppointment);
            return result;
        }
    }
}
