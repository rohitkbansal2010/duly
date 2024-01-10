// <copyright file="ClientExtensions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;
extern alias stu3;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System.Collections.Generic;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir;
using STU3 = stu3::Hl7.Fhir;

namespace Duly.Clinic.Fhir.Adapter.Extensions.Fhir
{
    public static class ClientExtensions
    {
        /// <summary>
        /// Searches resources of type T with parameters q in client.
        /// </summary>
        /// <typeparam name="T">Type of searched resource.</typeparam>
        /// <param name="client">Fhir client that will be used for retrieval.</param>
        /// <param name="q">Search parameters.</param>
        /// <returns>Returns enumeration of entries of T.</returns>
        public static async Task<IEnumerable<T>> FindResourcesAsync<T>(this R4.Rest.FhirClient client, SearchParams q)
            where T : Resource
        {
            var result = await client.SearchAsync<T>(q);
            var list = new List<T>();
            while (result != null)
            {
                list.AddRange(result.ExtractResource<T>());
                result = await client.ContinueAsync(result);
            }

            return list.ToArray();
        }

        /// <summary>
        /// Finds resources by Search Params.
        /// </summary>
        /// <typeparam name="T">One of Fhir resource.</typeparam>
        /// <param name="client">Fhir client that will be used for retrieval.</param>
        /// <param name="q">Search parameters.</param>
        /// <returns>Returns items of <see cref="R4.Model.Bundle.EntryComponent"/>.</returns>
        public static async Task<R4.Model.Bundle.EntryComponent[]> FindResourceBundles<T>(this R4.Rest.FhirClient client, SearchParams q)
            where T : Resource
        {
            var result = await client.SearchAsync<T>(q);
            var componentsList = new List<R4.Model.Bundle.EntryComponent>();
            while (result != null)
            {
                componentsList.AddRange(result.ExtractEntryComponents());
                result = await client.ContinueAsync(result);
            }

            return componentsList.ToArray();
        }

        /// <summary>
        /// Finds resources by Search Params.
        /// </summary>
        /// <typeparam name="T">One of Fhir resource.</typeparam>
        /// <param name="client">Fhir client that will be used for retrieval.</param>
        /// <param name="q">Search parameters.</param>
        /// <returns>Returns items of <see cref="STU3.Model.Bundle.EntryComponent"/>.</returns>
        public static async Task<STU3.Model.Bundle.EntryComponent[]> FindResourceBundles<T>(this STU3.Rest.FhirClient client, SearchParams q)
            where T : Resource
        {
            var result = await client.SearchAsync<T>(q);
            var componentsList = new List<STU3.Model.Bundle.EntryComponent>();
            while (result != null)
            {
                componentsList.AddRange(result.ExtractEntryComponents());
                result = await client.ContinueAsync(result);
            }

            return componentsList.ToArray();
        }
    }
}