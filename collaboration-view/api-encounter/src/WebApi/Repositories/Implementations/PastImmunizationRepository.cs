// -----------------------------------------------------------------------
// <copyright file="PastImmunizationRepository.cs" company="Duly Health and Care">
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

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IPastImmunizationRepository"/>
    /// </summary>
    internal class PastImmunizationRepository : IPastImmunizationRepository
    {
        private readonly IEncounterContext _encounterContext;
        private readonly IMapper _mapper;
        private readonly IPatientsClient _patientsClient;

        public PastImmunizationRepository(
            IEncounterContext encounterContext,
            IMapper mapper,
            IPatientsClient patientsClient)
        {
            _encounterContext = encounterContext;
            _mapper = mapper;
            _patientsClient = patientsClient;
        }

        public async Task<IEnumerable<PastImmunization>> GetPastImmunizationsForPatientAsync(string patientId, PastImmunizationStatus[] pastImmunizationStatuses)
        {
            var epicImmunizationStatuses = _mapper.Map<IEnumerable<ImmunizationStatus>>(pastImmunizationStatuses);

            var epicImmunizations = await _patientsClient.ImmunizationsAsync(
                    patientId,
                    _encounterContext.GetXCorrelationId(),
                    epicImmunizationStatuses);

            return _mapper.Map<IEnumerable<PastImmunization>>(epicImmunizations);
        }
    }
}
