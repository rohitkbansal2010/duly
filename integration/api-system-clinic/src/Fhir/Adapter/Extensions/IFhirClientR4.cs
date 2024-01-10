// <copyright file="IFhirClientR4.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System.Collections.Generic;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir;

namespace Duly.Clinic.Fhir.Adapter.Extensions
{
    /// <summary>
    /// Client to communicate with FHIR resources.
    /// </summary>
    public interface IFhirClientR4
    {
        /// <summary>
        /// Searches resources of type T with parameters q in client.
        /// </summary>
        /// <typeparam name="T">Type of searched resource.</typeparam>
        /// <param name="q">Search parameters.</param>
        /// <returns>Returns enumeration of entries of T.</returns>
        Task<IEnumerable<T>> FindResourcesAsync<T>(SearchParams q)
            where T : Resource;

        /// <summary>
        /// Retrieve resource of <see cref="T"/>> by <see cref="id"/>.
        /// </summary>
        /// <typeparam name="T">Type of the required resource.</typeparam>
        /// <param name="id">Id of the resource to search.</param>
        /// <returns>Returns search entries.</returns>
        Task<R4.Model.Bundle> SearchByIdAsync<T>(string id)
            where T : Resource, new();

        /// <summary>
        /// Finds Care teams by Search Params.
        /// </summary>
        /// <param name="q">Search parameters.</param>
        /// <returns>Returns search entries.</returns>
        Task<R4.Model.Bundle.EntryComponent[]> FindCareTeamsAsync(SearchParams q);

        /// <summary>
        /// Finds Encounters by Search Params.
        /// </summary>
        /// <param name="q">Search parameters.</param>
        /// <returns>Returns search entries.</returns>
        Task<R4.Model.Bundle.EntryComponent[]> FindEncountersAsync(SearchParams q);

        /// <summary>
        /// Finds Practitioner by Search Params.
        /// </summary>
        /// <param name="q">Search parameters.</param>
        /// <returns>Returns practitioners.</returns>
        Task<R4.Model.Bundle.EntryComponent[]> FindPractitionersAsync(SearchParams q);

        /// <summary>
        /// Finds Practitioner Roles by Search Params.
        /// </summary>
        /// <param name="q">Search parameters.</param>
        /// <returns>Returns practitioners with encounters.</returns>
        Task<R4.Model.Bundle.EntryComponent[]> FindPractitionerRolesAsync(SearchParams q);

        /// <summary>
        /// Retrieve patients by Search Params.
        /// </summary>
        /// <param name="q">Search parameters.</param>
        /// <returns>Returns search entries.</returns>
        Task<R4.Model.Bundle.EntryComponent[]> FindPatientsAsync(SearchParams q);

        /// <summary>
        /// Retrieve observations by Search Params.
        /// </summary>
        /// <param name="q">Search parameters.</param>
        /// <returns>Returns search entries.</returns>
        Task<R4.Model.Bundle.EntryComponent[]> FindObservationsAsync(SearchParams q);

        /// <summary>
        /// Retrieve DiagnosticReports by Search Params.
        /// </summary>
        /// <param name="q">Search parameters.</param>
        /// <returns>Returns search entries.</returns>
        Task<R4.Model.Bundle.EntryComponent[]> FindDiagnosticReportsAsync(SearchParams q);

        /// <summary>
        /// Retrieve ServiceRequest by Search Params.
        /// </summary>
        /// <param name="q">Search parameters.</param>
        /// <returns>Returns search entries.</returns>
        Task<R4.Model.Bundle.EntryComponent[]> FindServiceRequestAsync(SearchParams q);

        /// <summary>
        /// Retrieve Medication request by Search Params.
        /// </summary>
        /// <param name="q">Search parameters.</param>
        /// <returns>Returns search entries.</returns>
        public Task<R4.Model.Bundle.EntryComponent[]> FindMedicationRequestAsync(SearchParams q);
    }
}