// <copyright file="IImmunizationAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using System.Collections.Generic;
using System.Threading.Tasks;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// Provides Immunizations.
    /// </summary>
    public interface IImmunizationAdapter
    {
        /// <summary>
        /// Finds immunizations for Patient, optionally filters by Immunization Status.
        /// </summary>
        /// <param name="criteria">Search criteria.</param>
        /// <returns>Collection of Immunizations for the specified patient.</returns>
        Task<IEnumerable<R4.Immunization>> FindImmunizationsForPatientAsync(ImmunizationSearchCriteria criteria);
    }
}