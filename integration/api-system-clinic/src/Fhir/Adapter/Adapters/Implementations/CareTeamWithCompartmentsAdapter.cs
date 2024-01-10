// <copyright file="CareTeamWithCompartmentsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Extensions;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Adapters.Implementations
{
    /// <summary>
    /// <inheritdoc cref="ICareTeamWithCompartmentsAdapter"/>
    /// </summary>
    internal class CareTeamWithCompartmentsAdapter : ICareTeamWithCompartmentsAdapter
    {
        /// <summary>
        /// Compartments which can be retrieved with the main resource.
        /// </summary>
        private static readonly string[] Includes = { "CareTeam:member:Practitioner" };

        private readonly IFhirClientR4 _client;
        private readonly ICareTeamWithCompartmentsBuilder _builder;

        public CareTeamWithCompartmentsAdapter(IFhirClientR4 client, ICareTeamWithCompartmentsBuilder builder)
        {
            _client = client;
            _builder = builder;
        }

        public async Task<CareTeamWithParticipants[]> FindCareTeamsWithParticipantsAsync(CareTeamSearchCriteria careTeamSearchCriteria)
        {
            var searchParams = careTeamSearchCriteria.ToSearchParams().AddIncludes(Includes);
            var searchResult = await _client.FindCareTeamsAsync(searchParams);
            return await _builder.ExtractCareTeamsWithParticipantsAsync(searchResult, careTeamSearchCriteria.EndOfParticipation, careTeamSearchCriteria.CategoryCoding, true);
        }
    }
}
