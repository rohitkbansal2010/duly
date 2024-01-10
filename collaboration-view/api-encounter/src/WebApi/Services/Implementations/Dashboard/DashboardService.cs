// <copyright file="DashboardService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts.Dashboard;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.Dashboard;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations.Dashboard
{
    public class DashboardService : IDashboardService
    {

        private readonly IMapper _mapper;
        private readonly IDashboardRepository _repository;

        public DashboardService(IMapper mapper, IDashboardRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        public async Task<IEnumerable<PatientAndAppointmentCount>> GetPatientAndAppointmentCountAsync(string locationId)
        {
            var _requestPatientAndAppointmentCount = await _repository.GetPatientAndAppointmentCountAsync(locationId);
            var _responsePatientAndAppointmentCount = _mapper.Map<IEnumerable<PatientAndAppointmentCount>>(_requestPatientAndAppointmentCount);
            return _responsePatientAndAppointmentCount;
        }
    }
}
