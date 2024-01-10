// <copyright file="HealthConditionAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Adapters.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IHealthConditionAdapter"/>
    /// </summary>
    internal class HealthConditionAdapter : IHealthConditionAdapter
    {
        private readonly IFhirClientR4 _client;

        public HealthConditionAdapter(IFhirClientR4 client)
        {
            _client = client;
        }

        public async Task<IEnumerable<R4.Condition>> FindConditionsForPatientAsync(ConditionSearchCriteria criteria)
        {
            var searchResult = await _client.FindResourcesAsync<R4.Condition>(criteria.ToSearchParams());

            return searchResult;
        }
    }
}