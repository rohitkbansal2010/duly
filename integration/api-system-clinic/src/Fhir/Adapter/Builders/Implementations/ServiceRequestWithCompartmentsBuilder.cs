// <copyright file="ServiceRequestWithCompartmentsBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using System.Collections.Generic;
using System.Linq;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Builders.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IServicerequestWithCompartmentsBuilder"/>
    /// </summary>
    internal class ServiceRequestWithCompartmentsBuilder : IServicerequestWithCompartmentsBuilder
    {
        public ServiceRequestWithCompartments[] ExtractServiceRequestWithCompartments(IEnumerable<R4.Bundle.EntryComponent> searchResult)
        {
            var serviceRequest = searchResult.Select(component => component.Resource).OfType<R4.ServiceRequest>();
            return serviceRequest.Select(serviceRequest => new ServiceRequestWithCompartments
            {
                Resource = serviceRequest
            }).ToArray();
        }
    }
}