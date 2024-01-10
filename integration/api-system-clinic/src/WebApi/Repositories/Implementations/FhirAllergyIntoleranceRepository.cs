// <copyright file="FhirAllergyIntoleranceRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using AutoMapper;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Extensions;
using Hl7.Fhir.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IAllergyIntoleranceRepository"/>
    /// </summary>
    internal class FhirAllergyIntoleranceRepository : IAllergyIntoleranceRepository
    {
        private const string VerificationConfirmed = "confirmed";
        private const string CodingSystem = "http://terminology.hl7.org/CodeSystem/allergyintolerance-verification";
        private static readonly Coding _coding = new(CodingSystem, VerificationConfirmed);

        private readonly IAllergyIntoleranceAdapter _adapter;
        private readonly IMapper _mapper;

        public FhirAllergyIntoleranceRepository(IAllergyIntoleranceAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AllergyIntolerance>> GetConfirmedAllergyIntoleranceForPatientAsync(string patientId, ClinicalStatus clinicalStatus)
        {
            var searchCriteria = new SearchCriteria { PatientId = patientId, Status = clinicalStatus.ToString() };
            var fhirAllergyIntolerance = await _adapter.FindAllergyIntolerancesAsync(searchCriteria);
            var confirmedAndValid = GetConfirmedAndValid(fhirAllergyIntolerance);
            var systemAllergyIntolerance = _mapper.Map<IEnumerable<AllergyIntolerance>>(confirmedAndValid);
            return systemAllergyIntolerance;
        }

        private static IEnumerable<R4.AllergyIntolerance> GetConfirmedAndValid(IEnumerable<R4.AllergyIntolerance> fhirAllergyIntolerance)
        {
            return fhirAllergyIntolerance
                .Where(x => x.VerificationStatus.Coding.Contains(_coding, new CodingsComparer()))
                .Where(x => x.RecordedDate != null);
        }
    }
}
