// <copyright file="FhirEncounterRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IEncounterRepository"/>
    /// </summary>
    internal class FhirEncounterRepository : IEncounterRepository
    {
        private readonly IEncounterWithCompartmentsAdapter _adapter;
        private readonly IMapper _mapper;
        private readonly ILogger<FhirEncounterRepository> _logger;

        public FhirEncounterRepository(
            IEncounterWithCompartmentsAdapter adapter,
            IMapper mapper,
            ILogger<FhirEncounterRepository> logger)
        {
            _adapter = adapter;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<Encounter>> GetEncountersForSiteByDateAsync(string siteId, DateTime date)
        {
            var fhirEncounters = await _adapter.FindEncountersWithCompartmentsAsync(new SearchCriteria { SiteId = siteId, StartDateTime = date });
            var systemEncounters = _mapper.Map<Encounter[]>(fhirEncounters);
            if (systemEncounters.Length == 0)
            {
                _logger.LogInformation("Could not find any encounters for the site for the date");
            }

            return systemEncounters;
        }
    }
}