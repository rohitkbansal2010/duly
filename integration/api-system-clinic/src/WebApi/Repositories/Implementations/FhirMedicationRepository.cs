// <copyright file="FhirMedicationRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IMedicationRepository"/>
    /// </summary>
    internal class FhirMedicationRepository : IMedicationRepository
    {
        private readonly IMedicationStatementWithCompartmentsAdapter _adapter;
        private readonly IMapper _mapper;

        public FhirMedicationRepository(IMedicationStatementWithCompartmentsAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Medication>> FindMedicationsForPatientAsync(string patientId, MedicationStatus? status = null, MedicationCategory[] medicationCategories = null)
        {
            var searchCriteria = new MedicationSearchCriteria
            {
                PatientId = patientId,
                Status = status?.ToString(),
                Categories = medicationCategories
                    ?.Select(c => c.ToString())
                    .ToArray()
            };
            var fhirMedications = await _adapter.FindMedicationsWithCompartmentsAsync(searchCriteria);
            var systemMedications = _mapper.Map<IEnumerable<Medication>>(fhirMedications);
            return systemMedications;
        }

        public async Task<IEnumerable<Medication>> FindMedicationsRequestForPatientAsync(string patientId, string appointmentId, MedicationStatus? status = null, MedicationCategory[] medicationCategories = null)
        {
            var searchCriteria = new MedicationSearchCriteria
            {
                PatientId = patientId,
                Status = status?.ToString(),
                Categories = medicationCategories
                    ?.Select(c => c.ToString())
                    .ToArray()
            };
            var fhirMedications = await _adapter.FindMedicationsRequestWithCompartmentsAsync(searchCriteria);
            //var systemMedications = _mapper.Map<IEnumerable<Medication>>(fhirMedications);

            List<Medication> result = new List<Medication>();

            foreach (var item in fhirMedications)
            {
                if (item.Resource.Encounter.Identifier != null && item.Resource.Encounter.Identifier.Value == appointmentId)
                {
                    result.Add(_mapper.Map<Medication>(item));
                }
            }

            return result;
        }
    }
}