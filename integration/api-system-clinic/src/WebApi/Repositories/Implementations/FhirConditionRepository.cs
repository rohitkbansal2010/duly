// <copyright file="FhirConditionRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using AutoMapper;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IConditionRepository"/>
    /// </summary>
    internal class FhirConditionRepository : IConditionRepository
    {
        private const string ProblemCategory = "problem-list-item";

        private readonly IHealthConditionAdapter _adapter;
        private readonly IMapper _mapper;

        public FhirConditionRepository(IHealthConditionAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Condition>> FindProblemsForPatientAsync(string patientId, ConditionClinicalStatus[] status)
        {
            return await FindConditionsForPatient(patientId, ConditionCategory.Problem,  status);
        }

        private static ConditionSearchCriteria BuildSearchCriteria(string patientId, ConditionCategory category, ConditionClinicalStatus[] statuses)
        {
            var searchCriteria = new ConditionSearchCriteria { PatientId = patientId };

            if (category == ConditionCategory.Problem)
            {
                searchCriteria.Categories = new[] { ProblemCategory };
            }

            searchCriteria.Statuses = statuses.Select(MapStatuses).ToArray();

            return searchCriteria;
        }

        private static R4.Condition.ConditionClinicalStatusCodes MapStatuses(ConditionClinicalStatus status)
        {
            return status switch
            {
                ConditionClinicalStatus.Active => R4.Condition.ConditionClinicalStatusCodes.Active,
                ConditionClinicalStatus.Inactive => R4.Condition.ConditionClinicalStatusCodes.Inactive,
                ConditionClinicalStatus.Resolved => R4.Condition.ConditionClinicalStatusCodes.Resolved,
                _ => throw new ConceptNotMappedException("Missing mapping for ConditionClinicalStatus")
            };
        }

        private async Task<IEnumerable<Condition>> FindConditionsForPatient(string patientId, ConditionCategory category, ConditionClinicalStatus[] status)
        {
            ConditionSearchCriteria searchCriteria = BuildSearchCriteria(patientId, category, status);
            var fhirConditions = await _adapter.FindConditionsForPatientAsync(searchCriteria);
            var systemConditions = _mapper.Map<IEnumerable<Condition>>(fhirConditions);
            return systemConditions;
        }
    }
}
