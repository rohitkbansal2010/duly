// -----------------------------------------------------------------------
// <copyright file="AllergyIntoleranceRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using AutoMapper;
using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using AllergyIntolerance = Duly.CollaborationView.Encounter.Api.Repositories.Models.AllergyIntolerance;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IAllergyIntoleranceRepository"/>
    /// </summary>
    internal class AllergyIntoleranceRepository : IAllergyIntoleranceRepository
    {
        private readonly IEncounterContext _encounterContext;
        private readonly IMapper _mapper;
        private readonly IAllergyIntoleranceClient _allergyIntoleranceClient;

        public AllergyIntoleranceRepository(
            IEncounterContext encounterContext,
            IMapper mapper,
            IAllergyIntoleranceClient allergyIntoleranceClient)
        {
            _encounterContext = encounterContext;
            _mapper = mapper;
            _allergyIntoleranceClient = allergyIntoleranceClient;
        }

        public async Task<IEnumerable<AllergyIntolerance>> GetAllergyIntolerancesForPatientAsync(
            string patientId,
            AllergyIntoleranceClinicalStatus status)
        {
            var systemStatus = _mapper.Map<ClinicalStatus>(status);

            var allergyIntolerances = await _allergyIntoleranceClient.ConfirmedAsync(
                patientId,
                systemStatus,
                _encounterContext.GetXCorrelationId());

            return _mapper.Map<IEnumerable<AllergyIntolerance>>(allergyIntolerances);
        }
    }
}
