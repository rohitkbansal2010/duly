// <copyright file="NgdpImmunizationRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Api.Repositories.Interfaces;
using Duly.Ngdp.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IImmunizationRepository"/>
    /// </summary>
    internal class NgdpImmunizationRepository : IImmunizationRepository
    {
        private readonly IImmunizationAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpImmunizationRepository(IImmunizationAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Immunization>> GetImmunizationsForSpecificPatientAsync(string patientId, IEnumerable<DueStatus> includedDueStatuses)
        {
            var searchCriteria = BuildAppointmentSearchCriteria(patientId, includedDueStatuses);

            var ngdpImmunizations = await _adapter.FindImmunizationsAsync(searchCriteria);

            var systemImmunizations = _mapper.Map<IEnumerable<Immunization>>(ngdpImmunizations);

            return systemImmunizations;
        }

        private static AdapterModels.ImmunizationSearchCriteria BuildAppointmentSearchCriteria(string patientId, IEnumerable<DueStatus> includedDueStatuses)
        {
            var searchCriteria = new AdapterModels.ImmunizationSearchCriteria
            {
                PatientId = patientId,
                IncludedDueStatusesIds = includedDueStatuses.Select(status => (int)status).ToArray()
            };

            return searchCriteria;
        }
    }
}