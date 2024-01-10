// <copyright file="ServiceRequestWithCompartmentsAdapter.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="IServiceRequestWithCompartmentsAdapter"/>
    /// </summary>
    internal class ServiceRequestWithCompartmentsAdapter : IServiceRequestWithCompartmentsAdapter
    {
        private readonly IFhirClientR4 _client;
        private readonly IServicerequestWithCompartmentsBuilder _builder;

        public ServiceRequestWithCompartmentsAdapter(IFhirClientR4 client, IServicerequestWithCompartmentsBuilder builder)
        {
            _client = client;
            _builder = builder;
        }

        public async Task<IEnumerable<ServiceRequestWithCompartments>> FindLabImagingOrdersAsync(ServicerequestSearchCriteria serviceRequestSearchCriteria)
        {

            var searchParams = serviceRequestSearchCriteria.ToSearchParams();
            var searchResult = await _client.FindServiceRequestAsync(searchParams);
            if (searchResult.Length == 0)
            {
                return null;
            }

            return _builder.ExtractServiceRequestWithCompartments(searchResult);
        }
    }
}