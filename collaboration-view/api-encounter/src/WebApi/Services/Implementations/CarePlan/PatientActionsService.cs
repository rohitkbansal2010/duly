// <copyright file="PatientActionsService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="IPatientActionsService"/>
    /// </summary>
    internal class PatientActionsService : IPatientActionsService
    {
        private readonly IMapper _mapper;
        private readonly IPatientActionsRepository _repository;
        public PatientActionsService(IMapper mapper, IPatientActionsRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<GetPatientActions>> GetPatientActionsByPatientTargetIdAsync(long patientTargetId)
        {
            var getPatientActionsResponse = await _repository.GetPatientActionsByPatientTargetIdAsync(patientTargetId);
            var result = _mapper.Map<IEnumerable<GetPatientActions>>(getPatientActionsResponse);
            return result;
        }

        public async Task<long> PostPatientActionsAsync(IEnumerable<PatientActions> request)
        {
            var requestPatientActions = _mapper.Map<IEnumerable<Models.PatientActions>>(request);
            var responsePatientActions = await _repository.PostPatientActionsAsync(requestPatientActions);
            return responsePatientActions;
        }

        public async Task<long> UpdateActionProgressAsync(UpdateActionProgress request, long patientActionId)
        {
            var requestPatientActions = _mapper.Map<Models.UpdateActionProgress>(request);
            var responsePatientActions = await _repository.UpdateActionProgressAsync(requestPatientActions, patientActionId);
            return responsePatientActions;
        }

        public async Task<GetPatientActionStats> GetPatientActionStatsAsync(long patientPlanId)
        {
            var response = await _repository.GetPatientActionStatsAsync(patientPlanId);
            var result = _mapper.Map<GetPatientActionStats>(response);
            return result;
        }
    }
}