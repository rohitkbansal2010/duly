// <copyright file="FhirPractitionerGeneralInfoRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IPractitionerGeneralInfoRepository"/>
    /// </summary>
    internal class FhirPractitionerGeneralInfoRepository : IPractitionerGeneralInfoRepository
    {
        private readonly IPractitionerWithCompartmentsAdapter _adapter;
        private readonly IMapper _mapper;

        public FhirPractitionerGeneralInfoRepository(
            IPractitionerWithCompartmentsAdapter adapter,
            IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PractitionerGeneralInfo>> GetPractitionersBySiteIdAsync(string siteId)
        {
            var fhirPractitioners = await _adapter.FindPractitionersWithRolesAsync(new SearchCriteria { SiteId = siteId });
            var systemPractitioners = _mapper.Map<IEnumerable<PractitionerGeneralInfo>>(fhirPractitioners);
            return systemPractitioners;
        }

        public async Task<IEnumerable<PractitionerGeneralInfo>> GetPractitionersByIdentifiersAsync(string[] identifiers)
        {
            var fhirPractitioners = await _adapter.FindPractitionersByIdentifiersAsync(identifiers);
            var systemPractitioners = _mapper.Map<IEnumerable<PractitionerGeneralInfo>>(fhirPractitioners);
            return systemPractitioners;
        }
    }
}
