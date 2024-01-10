// <copyright file="NgdpPharmacyRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Common.Infrastructure.Exceptions;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Api.Repositories.Interfaces;
using Duly.Ngdp.Api.Repositories.Mappings.Converters;
using Duly.Ngdp.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IPharmacyRepository"/>
    /// </summary>
    internal class NgdpPharmacyRepository : IPharmacyRepository
    {
        private readonly IPharmacyAdapter _adapter;
        private readonly IMapper _mapper;
        private readonly ITimeZoneConverter _timeZoneConverter;

        public NgdpPharmacyRepository(IPharmacyAdapter adapter, IMapper mapper, ITimeZoneConverter timeZoneConverter)
        {
            _adapter = adapter;
            _mapper = mapper;
            _timeZoneConverter = timeZoneConverter;
        }

        public async Task<Pharmacy> GetPreferredPharmacyByPatientIdAsync(string patientId)
        {
            var ngdpPharmacy = await _adapter.FindPreferredPharmacyByPatientIdAsync(patientId);

            var ngdpResponsePharmacy = _mapper.Map<Pharmacy>(ngdpPharmacy);

            return ngdpResponsePharmacy;
        }
    }
}