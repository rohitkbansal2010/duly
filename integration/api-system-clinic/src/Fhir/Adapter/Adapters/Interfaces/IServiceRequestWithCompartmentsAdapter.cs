// <copyright file="IServiceRequestWithCompartmentsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// Adapter to work on Service request with compartments.
    /// </summary>
    public interface IServiceRequestWithCompartmentsAdapter
    {
        /// <summary>
        /// Gets Service request and its compartments.
        /// </summary>
        /// <param name="searchCriteria">SearchCriteria for lab and Imaging Orders.</param>
        /// <returns><see cref="ServiceRequestWithCompartments"/> item or null if necessary item is not found.</returns>
        Task<IEnumerable<ServiceRequestWithCompartments>> FindLabImagingOrdersAsync(ServicerequestSearchCriteria searchCriteria);
    }
}