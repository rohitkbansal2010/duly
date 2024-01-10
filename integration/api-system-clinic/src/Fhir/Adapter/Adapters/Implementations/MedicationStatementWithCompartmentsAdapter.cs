// <copyright file="MedicationStatementWithCompartmentsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Adapters.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IMedicationStatementWithCompartmentsAdapter"/>
    /// </summary>
    internal class MedicationStatementWithCompartmentsAdapter : IMedicationStatementWithCompartmentsAdapter
    {
        /// <summary>
        /// Compartments which can be retrieved with the main resource.
        /// </summary>
        private static readonly string[] Includes = { "MedicationStatement:informationsource:Practitioner", "MedicationStatement:reasonreference" };
        private static readonly string[] IncludesRequest = { "MedicationRequest:requester:Practitioner"};

        private readonly IFhirClientSTU3 _client;
        private readonly IFhirClientR4 _r4client;
        private readonly IMedicationRequestWithCompartmentsBuilder _r4builder;
        private readonly IMedicationStatementWithCompartmentsBuilder _builder;
        private readonly IPractitionerWithRolesBuilder _practitionerWithRolesBuilder;

        public MedicationStatementWithCompartmentsAdapter(
            IFhirClientSTU3 client,
            IFhirClientR4 r4client,
            IMedicationStatementWithCompartmentsBuilder builder,
            IMedicationRequestWithCompartmentsBuilder r4builder,
            IPractitionerWithRolesBuilder practitionerWithRolesBuilder)
        {
            _client = client;
            _r4client = r4client;
            _r4builder = r4builder;
            _builder = builder;
            _practitionerWithRolesBuilder = practitionerWithRolesBuilder;
        }

        public async Task<IEnumerable<MedicationStatementWithCompartments>> FindMedicationsWithCompartmentsAsync(MedicationSearchCriteria criteria)
        {
            var searchParams = criteria.ToSearchParams().AddIncludes(Includes);
            var searchResult = await _client.FindMedicationStatementsAsync(searchParams);
            var practitionersWithRoles = await _practitionerWithRolesBuilder.RetrievePractitionerWithRolesAsync(searchResult);

            return _builder.ExtractMedicationWithCompartments(searchResult, practitionersWithRoles);
        }

        public async Task<IEnumerable<MedicationRequestWithCompartments>> FindMedicationsRequestWithCompartmentsAsync(MedicationSearchCriteria criteria)
        {
            var searchParams = criteria.ToSearchParams().AddIncludes(IncludesRequest);
            var searchResult = await _r4client.FindMedicationRequestAsync(searchParams);
            var practitionersWithRoles = await _practitionerWithRolesBuilder.RetrievePractitionerWithRolesAsync(searchResult);

            return _r4builder.ExtractMedicationRequestWithCompartments(searchResult, practitionersWithRoles);
        }
    }
}
