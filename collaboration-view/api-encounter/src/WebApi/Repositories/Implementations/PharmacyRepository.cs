// -----------------------------------------------------------------------
// <copyright file="PharmacyRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// --------------

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using Duly.Ngdp.Api.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IPharmacyRepository/>
    /// </summary>
    internal class PharmacyRepository : IPharmacyRepository
    {
        private readonly IEncounterContext _encounterContext;
        private readonly IMapper _mapper;
        private readonly IClient _pharmacyClient;

        public PharmacyRepository(
           IEncounterContext encounterContext,
           IMapper mapper,
           IClient pharmacyClient)
        {
            _encounterContext = encounterContext;
            _mapper = mapper;
            _pharmacyClient = pharmacyClient;
        }

        public async Task<Models.Pharmacy> GeTPreferredPharmacyByPatientIdAsync(string patientId)
        {
            var pharmacyDetails = await _pharmacyClient.PharmacyAsync(patientId, _encounterContext.GetXCorrelationId());
            var result = _mapper.Map<Models.Pharmacy>(pharmacyDetails);
            return result;
        }
    }
}
