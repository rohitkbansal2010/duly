// <copyright file="IAllergyIntoleranceAdapter.cs" company="Duly Health and Care">
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
    /// Adapter to work on AllergyIntolerance.
    /// </summary>
    public interface IAllergyIntoleranceAdapter
    {
        /// <summary>
        /// Finds all suitable allergy intolerances.
        /// </summary>
        /// <param name="criteria">Search criteria. Should correctly work with PatientId and Status.</param>
        /// <returns>All data that satisfies search criteria.</returns>
        Task<IEnumerable<R4.AllergyIntolerance>> FindAllergyIntolerancesAsync(SearchCriteria criteria);
    }
}
