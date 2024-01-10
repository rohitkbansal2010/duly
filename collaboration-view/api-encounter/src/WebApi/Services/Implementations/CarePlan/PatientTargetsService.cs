// <copyright file="PatientTargetsService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="IPatientTargetsService"/>
    /// </summary>
    public class PatientTargetsService : IPatientTargetsService
    {
        private readonly IMapper _mapper;
        private readonly IPatientTargetsRepository _repository;

        public PatientTargetsService(IMapper mapper, IPatientTargetsRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<GetPatientTargets>> GetPatientTargetsByPatientPlanIdAsync(long patientPlanId)
        {
            var patientTargetsResponse = await _repository.GetPatientTargetsByPatientPlanIdAsync(patientPlanId);
            var result = _mapper.Map<IEnumerable<GetPatientTargets>>(patientTargetsResponse);
            return result;
        }

        public async Task<long> PostPatientTargetsAsync(PatientTargets request)
        {
            var mappedRequest = _mapper.Map<Repositories.Models.CarePlan.PatientTargets>(request);
            var result = await _repository.PostPatientTargetsAsync(mappedRequest);
            return result;
        }

        public async Task<int> DeletePatientTargetAsync(long id)
        {
            var result = await _repository.DeletePatientTargetAsync(id);
            return result;
        }

        public async Task<long> UpdatePatientTargetReviewStatusAsync(UpdatePatientTargetReviewStatus request, long patientTargetId)
        {
            var mappedRequest = _mapper.Map<Repositories.Models.CarePlan.UpdatePatientTargetReviewStatus>(request);
            var result = await _repository.UpdatePatientTargetReviewStatusAsync(mappedRequest, patientTargetId);
            return result;
        }

        public async Task<long> UpdatePatientTargetsAsync(UpdatePatientTargets request, long patientTargetId)
        {
            var mappedRequest = _mapper.Map<Repositories.Models.CarePlan.UpdatePatientTargets>(request);
            var result = await _repository.UpdatePatientTargetsAsync(mappedRequest, patientTargetId);
            return result;
        }
    }
}