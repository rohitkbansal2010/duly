// <copyright file="ConditionService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan;
using Duly.Common.Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="IConditionService"/>
    /// </summary>
    public class ConditionService : IConditionService
    {
        private readonly IMapper _mapper;
        private readonly IConditionRepository _repository;

        public ConditionService(IMapper mapper, IConditionRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<GetConditionResponse>> GetConditions()
        {
            List<GetConditionResponse> listOfConditions = new List<GetConditionResponse>();
            var result = await _repository.GetConditions();
            if (result == null)
            {
                throw new EntityNotFoundException("Conditions not found.");
            }

            var conditionResponse = _mapper.Map<IEnumerable<Condition>>(result);
            foreach (var item in conditionResponse)
            {
                listOfConditions.Add(new GetConditionResponse
                {
                    Id = item.ConditionId,
                    Condition = item.ConditionDisplayName
                });
            }

            return listOfConditions;
        }
    }
}