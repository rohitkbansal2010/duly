// <copyright file="FhirImmunizationRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IImmunizationRepository"/>
    /// </summary>
    internal class FhirImmunizationRepository : IImmunizationRepository
    {
        private readonly IImmunizationAdapter _adapter;
        private readonly IMapper _mapper;

        public FhirImmunizationRepository(IImmunizationAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Immunization>> FindImmunizationsForPatientAsync(string patientId, ImmunizationStatus[] immunizationStatuses = null)
        {
            var searchCriteria = BuildImmunizationSearchCriteria(patientId, immunizationStatuses);

            var fhirImmunizations = await _adapter.FindImmunizationsForPatientAsync(searchCriteria);

            var systemImmunizations = _mapper.Map<IEnumerable<Immunization>>(fhirImmunizations);

            return systemImmunizations;
        }

        private static ImmunizationSearchCriteria BuildImmunizationSearchCriteria(
            string patientId,
            IEnumerable<ImmunizationStatus> immunizationStatuses)
        {
            var searchCriteria = new ImmunizationSearchCriteria
            {
                PatientId = patientId,
                Statuses = BuildStatuses(immunizationStatuses)
            };
            return searchCriteria;
        }

        private static string[] BuildStatuses(IEnumerable<ImmunizationStatus> immunizationStatuses)
        {
            return immunizationStatuses?
                .Select(ConvertImmunizationStatus)
                .ToArray();
        }

        private static string ConvertImmunizationStatus(ImmunizationStatus immunizationStatus)
        {
            return immunizationStatus switch
            {
                ImmunizationStatus.Completed => "completed",
                ImmunizationStatus.EnteredInError => "entered-in-error",
                ImmunizationStatus.NotDone => "not-done",
                _ => throw new ArgumentOutOfRangeException(nameof(immunizationStatus))
            };
        }
    }
}