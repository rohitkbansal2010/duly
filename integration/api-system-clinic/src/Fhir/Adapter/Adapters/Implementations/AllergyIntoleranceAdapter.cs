// <copyright file="AllergyIntoleranceAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Exceptions;
using Duly.Clinic.Fhir.Adapter.Extensions;
using Hl7.Fhir.Rest;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Threading.Tasks;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Adapters.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IAllergyIntoleranceAdapter"/>
    /// </summary>
    internal class AllergyIntoleranceAdapter : IAllergyIntoleranceAdapter
    {
        private readonly IFhirClientR4 _client;

        public AllergyIntoleranceAdapter(IFhirClientR4 client)
        {
            _client = client;
        }

        public async Task<IEnumerable<R4.AllergyIntolerance>> FindAllergyIntolerancesAsync(SearchCriteria criteria)
        {
            var searchParams = BySearchCriteriaForAllergyIntolerance(criteria);
            var searchResult = await _client.FindResourcesAsync<R4.AllergyIntolerance>(searchParams);

            return searchResult;
        }

        private static SearchParams BySearchCriteriaForAllergyIntolerance(SearchCriteria criteria)
        {
            var searchParams = new SearchParams();

            if (criteria.PatientId.IsNullOrEmpty())
            {
                throw new MandatoryQueryParameterMissingException("AllergyIntolerance: patientId is missing.");
            }

            if (criteria.Status.IsNullOrEmpty())
            {
                throw new MandatoryQueryParameterMissingException("AllergyIntolerance: clinical-status is missing.");
            }

            searchParams.ByPatientId(criteria.PatientId);
            searchParams.ByClinicalStatus(criteria.Status.ToLower());

            return searchParams;
        }
    }
}
