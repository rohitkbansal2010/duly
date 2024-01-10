// <copyright file="ObservationWithCompartmentsAdapter.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="IObservationWithCompartmentsAdapter"/>
    /// </summary>
    internal class ObservationWithCompartmentsAdapter : IObservationWithCompartmentsAdapter
    {
        private readonly IFhirClientR4 _client;
        private readonly IObservationWithCompartmentsBuilder _builder;

        public ObservationWithCompartmentsAdapter(IFhirClientR4 client, IObservationWithCompartmentsBuilder builder)
        {
            _client = client;
            _builder = builder;
        }

        public async Task<IEnumerable<ObservationWithCompartments>> FindObservationsWithCompartmentsAsync(ObservationSearchCriteria searchCriteria)
        {
            var searchResult = await _client.FindObservationsAsync(searchCriteria.ToSearchParams());

            return _builder.ExtractObservations(searchResult);
        }
    }
}
