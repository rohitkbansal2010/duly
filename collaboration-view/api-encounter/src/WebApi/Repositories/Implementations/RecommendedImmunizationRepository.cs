// <copyright file="RecommendedImmunizationRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.Ngdp.Api.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IRecommendedImmunizationRepository"/>
    /// </summary>
    internal class RecommendedImmunizationRepository : IRecommendedImmunizationRepository
    {
        private readonly IEncounterContext _encounterContext;
        private readonly IMapper _mapper;
        private readonly IPatientsClient _patientsClient;

        public RecommendedImmunizationRepository(
            IEncounterContext encounterContext,
            IMapper mapper,
            IPatientsClient patientsClient)
        {
            _encounterContext = encounterContext;
            _mapper = mapper;
            _patientsClient = patientsClient;
        }

        public async Task<IEnumerable<RecommendedImmunization>> GetRecommendedImmunizationsForPatientAsync(
            string patientId,
            RecommendedImmunizationStatus[] recommendedImmunizationStatuses)
        {
            var dueStatuses = _mapper.Map<IEnumerable<DueStatus>>(recommendedImmunizationStatuses);

            var ngdpImmunizations = await _patientsClient.ImmunizationsAsync(
                patientId,
                dueStatuses,
                _encounterContext.GetXCorrelationId());

            return _mapper.Map<IEnumerable<RecommendedImmunization>>(ngdpImmunizations);
        }
    }
}