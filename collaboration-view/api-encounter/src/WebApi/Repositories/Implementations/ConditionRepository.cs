// -----------------------------------------------------------------------
// <copyright file="ConditionRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using AutoMapper;
using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IConditionRepository"/>
    /// </summary>
    internal class ConditionRepository : IConditionRepository
    {
        private readonly IEncounterContext _encounterContext;
        private readonly IMapper _mapper;
        private readonly IConditionsClient _conditionsClient;

        public ConditionRepository(
            IEncounterContext encounterContext,
            IMapper mapper,
            IConditionsClient conditionsClient)
        {
            _encounterContext = encounterContext;
            _mapper = mapper;
            _conditionsClient = conditionsClient;
        }

        public async Task<IEnumerable<Models.Condition>> GetConditionsForPatientAsync(string patientId, Models.ConditionClinicalStatus[] conditionClinicalStatuses)
        {
            var statuses = _mapper.Map<ConditionClinicalStatus[]>(conditionClinicalStatuses);

            var conditions = await _conditionsClient.ProblemsAsync(
                patientId,
                statuses,
                _encounterContext.GetXCorrelationId());

            return _mapper.Map<IEnumerable<Models.Condition>>(conditions);
        }
    }
}
