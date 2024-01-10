// <copyright file="PatientPlanService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="IPatientPlanService"/>
    /// </summary>
    internal class PatientPlanService : IPatientPlanService
    {
        private readonly IMapper _mapper;
        private readonly IPatientPlanRepository _repository;
        public PatientPlanService(IMapper mapper, IPatientPlanRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<int> PostPatientPlanAsync(PatientPlan request)
        {
            var requestPatientPlan = _mapper.Map<Models.PatientPlan>(request);
            var responsePatientPlan = await _repository.PostPatientPlanAsync(requestPatientPlan);
            return responsePatientPlan;
        }

        public async Task<bool> UpdatePatientPlanStatusByIdAsync(long id)
        {
            var responsePatientPlanStatus = await _repository.UpdatePatientPlanStatusByIdAsync(id);
            return responsePatientPlanStatus;
        }

        public async Task<GetPatientPlanByPatientId> GetPatientPlanByPatientIdAsync(string id)
        {
            var responsePatientPlan = await _repository.GetPatientPlanIdByPatientIdAsync(id);
            var result = _mapper.Map<GetPatientPlanByPatientId>(responsePatientPlan);
            return result;
        }

        public async Task<long> UpdateFlourishStatementAsync(UpdateFlourishStatementRequest request)
        {
            var patientPlanId = await _repository.UpdateFlourishStatementAsync(request);
            return patientPlanId;
        }

        public async Task<GetHealthPlanStats> GetHealthPlanStatsByPatientPlanIdAsync(long patientPlanId)
        {
            var count = await _repository.GetHealthPlanStatsByPatientPlanIdAsync(patientPlanId);
            var result = _mapper.Map<GetHealthPlanStats>(count);
            return result;
        }
    }
}