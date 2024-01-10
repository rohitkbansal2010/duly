// <copyright file="IHealthConditionAdapter.cs" company="Duly Health and Care">
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
    /// Provides Conditions.
    /// </summary>
    public interface IHealthConditionAdapter
    {
        /// <summary>
        /// Finds conditions for Patient.
        /// </summary>
        /// <param name="criteria">Search criteria.</param>
        /// <returns>Collection of Conditions of specified patient.</returns>
        Task<IEnumerable<R4.Condition>> FindConditionsForPatientAsync(ConditionSearchCriteria criteria);
    }
}