// <copyright file="IObservationWithCompartmentsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// Adapter to work on Observation with compartments.
    /// </summary>
    public interface IObservationWithCompartmentsAdapter
    {
        /// <summary>
        /// Gets Observations and their compartments.
        /// </summary>
        /// <param name="searchCriteria">Search criteria from list of parameters.</param>
        /// <returns>All data that satisfies search criteria.</returns>
        Task<IEnumerable<ObservationWithCompartments>> FindObservationsWithCompartmentsAsync(
            ObservationSearchCriteria searchCriteria);
    }
}
