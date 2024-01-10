// <copyright file="TargetActionsService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan;
using Duly.Common.Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="ITargetActionsService"/>
    /// </summary>
    public class TargetActionsService : ITargetActionsService
    {
        private readonly IMapper _mapper;
        private readonly ITargetActionsRepository _repository;

        public TargetActionsService(IMapper mapper, ITargetActionsRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<TargetActions>> GetTargetActionsByConditionIdAndTargetIdAsync(long conditionId, long targetId)
        {
            var result = await _repository.GetTargetActionsByConditionIdAndTargetIdAsync(conditionId, targetId);
            if (result == null)
            {
                throw new EntityNotFoundException(nameof(result));
            }

            var actionResponse = _mapper.Map<IEnumerable<TargetActions>>(result);
            return actionResponse;
        }
    }
}