// <copyright file="CustomActionsService.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="ICustomActionsService"/>
    /// </summary>
    internal class CustomActionsService : ICustomActionsService
    {
        private readonly IMapper _mapper;
        private readonly ICustomActionsRepository _repository;
        public CustomActionsService(IMapper mapper, ICustomActionsRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<long> PostCustomActionsAsync(CustomActions request)
        {
            var requestCustomActions = _mapper.Map<Models.CustomActions>(request);
            var responseCustomActions = await _repository.PostCustomActionsAsync(requestCustomActions);
            return responseCustomActions;
        }
    }
}