// <copyright file="IFhirClientSTU3.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias stu3;

using Hl7.Fhir.Rest;

using STU3 = stu3::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Extensions
{
    public interface IFhirClientSTU3
    {
        /// <summary>
        /// Retrieve medication statements by Search Params.
        /// </summary>
        /// <param name="q">Search parameters.</param>
        /// <returns>Returns search entries.</returns>
        System.Threading.Tasks.Task<STU3.Bundle.EntryComponent[]> FindMedicationStatementsAsync(SearchParams q);

        /// <summary>
        /// Retrieve practitioners roles by Search Params.
        /// </summary>
        /// <param name="q">Search parameters.</param>
        /// <returns>Returns search entries.</returns>
        System.Threading.Tasks.Task<STU3.Bundle.EntryComponent[]> FindPractitionerRolesAsync(SearchParams q);

        /// <summary>
        /// Finds Encounters by Search Params.
        /// </summary>
        /// <param name="q">Search parameters.</param>
        /// <returns>Returns search entries.</returns>
        System.Threading.Tasks.Task<STU3.Bundle.EntryComponent[]> FindEncountersAsync(SearchParams q);
    }
}