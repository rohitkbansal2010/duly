// <copyright file="PatientConditionsService.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="IPatientConditionsService"/>
    /// </summary>
    internal class PatientConditionsService : IPatientConditionsService
    {
        private readonly IMapper _mapper;
        private readonly IPatientConditionsRepository _repository;
        public PatientConditionsService(IMapper mapper, IPatientConditionsRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<long>> PostPatientConditionsAsync(PatientConditions request)
        {
            var requestPatientConditions = _mapper.Map<Models.PatientConditions>(request);
            var responsePatientConditions = await _repository.PostPatientConditionsAsync(requestPatientConditions);
            var response = _mapper.Map<IEnumerable<long>>(responsePatientConditions);
            return response;
        }

        public async Task<IEnumerable<GetPatientConditionByPatientPlanId>> GetConditionByPatientPlanId(long id)
        {
            var getConditionByIdResponse = await _repository.GetConditionByPatientPlanId(id);
            var result = _mapper.Map<List<GetPatientConditionByPatientPlanId>>(getConditionByIdResponse);
            return result;
        }
    }
}