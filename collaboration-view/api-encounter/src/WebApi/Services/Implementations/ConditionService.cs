// <copyright file="ConditionService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IConditionService"/>
    /// </summary>
    internal class ConditionService : IConditionService
    {
        private static readonly ConditionClinicalStatus[] ConditionClinicalStatusArray = { ConditionClinicalStatus.Active, ConditionClinicalStatus.Inactive, ConditionClinicalStatus.Resolved };
        private readonly IConditionRepository _repository;
        private readonly IMapper _mapper;

        public ConditionService(
            IMapper mapper,
            IConditionRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<HealthConditions> GetHealthConditionsByPatientIdAsync(string patientId)
        {
            var modelConditions = await _repository.GetConditionsForPatientAsync(
                patientId,
                ConditionClinicalStatusArray);
            var modelConditionsArray = modelConditions.ToArray();

            var modelCurrentConditions = modelConditionsArray.Where(x =>
                x.ClinicalStatus == ConditionClinicalStatus.Active);
            var modelPreviousConditions = modelConditionsArray.Where(x =>
                x.ClinicalStatus is ConditionClinicalStatus.Inactive or ConditionClinicalStatus.Resolved);

            var currentConditions = _mapper.Map<IEnumerable<HealthCondition>>(modelCurrentConditions);
            var previousConditions = _mapper.Map<IEnumerable<HealthCondition>>(modelPreviousConditions);

            return new()
            {
                CurrentHealthConditions = currentConditions
                    .OrderByDescending(x => x.Date)
                    .ThenBy(x => x.Title)
                    .ToArray(),
                PreviousHealthConditions = previousConditions
                    .OrderByDescending(x => x.Date)
                    .ThenBy(x => x.Title)
                    .ToArray()
            };
        }
    }
}
