// <copyright file="PractitionerWithCompartmentsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Extensions;
using Hl7.Fhir.Rest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Adapters.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IPractitionerWithCompartmentsAdapter"/>
    /// </summary>
    internal class PractitionerWithCompartmentsAdapter : IPractitionerWithCompartmentsAdapter
    {
        /// <summary>
        /// Compartments which can be retrieved with the main resource.
        /// </summary>
        private static readonly string[] Includes = { "PractitionerRole:practitioner:Practitioner" };

        private readonly IFhirClientR4 _client;
        private readonly IPractitionerWithRolesBuilder _builder;

        public PractitionerWithCompartmentsAdapter(IFhirClientR4 client, IPractitionerWithRolesBuilder builder)
        {
            _client = client;
            _builder = builder;
        }

        public async Task<IEnumerable<PractitionerWithRoles>> FindPractitionersWithRolesAsync(SearchCriteria criteria)
        {
            var searchParams = ToSearchParams(criteria).AddIncludes(Includes);
            var searchResult = await _client.FindPractitionerRolesAsync(searchParams);

            return _builder.ExtractPractitionerWithRoles(searchResult, true);
        }

        public async Task<IEnumerable<PractitionerWithRoles>> FindPractitionersByIdentifiersAsync(string[] identifiers)
        {
            var searchParams = new SearchParams().ByIdentifiers(identifiers);
            var searchResult = await _client.FindPractitionersAsync(searchParams);

            var practitionersWithRoles = await _builder.RetrievePractitionerWithRolesSafeAsync(searchResult);
            return practitionersWithRoles;
        }

        private static SearchParams ToSearchParams(SearchCriteria criteria)
        {
            var searchParams = new SearchParams();

            if (criteria.SiteId != null)
            {
                searchParams.BySiteId(criteria.SiteId);
            }

            return searchParams;
        }
    }
}
