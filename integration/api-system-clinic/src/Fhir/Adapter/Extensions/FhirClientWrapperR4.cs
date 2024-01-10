// <copyright file="FhirClientWrapperR4.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using Duly.Clinic.Fhir.Adapter.Extensions.Fhir;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System.Collections.Generic;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir;

namespace Duly.Clinic.Fhir.Adapter.Extensions
{
    internal class FhirClientWrapperR4 : IFhirClientR4
    {
        private readonly R4.Rest.FhirClient _client;

        public FhirClientWrapperR4(R4.Rest.FhirClient client)
        {
            _client = client;
        }

        public Task<IEnumerable<T>> FindResourcesAsync<T>(SearchParams q)
            where T : Resource
        {
            return _client.FindResourcesAsync<T>(q);
        }

        public Task<R4.Model.Bundle> SearchByIdAsync<T>(string id)
            where T : Resource, new()
        {
            return _client.SearchByIdAsync<T>(id);
        }

        public Task<R4.Model.Bundle.EntryComponent[]> FindCareTeamsAsync(SearchParams q)
        {
            return _client.FindResourceBundles<R4.Model.CareTeam>(q);
        }

        public Task<R4.Model.Bundle.EntryComponent[]> FindEncountersAsync(SearchParams q)
        {
            return _client.FindResourceBundles<R4.Model.Encounter>(q);
        }

        public Task<R4.Model.Bundle.EntryComponent[]> FindPractitionersAsync(SearchParams q)
        {
            return _client.FindResourceBundles<R4.Model.Practitioner>(q);
        }

        public Task<R4.Model.Bundle.EntryComponent[]> FindPractitionerRolesAsync(SearchParams q)
        {
            return _client.FindResourceBundles<R4.Model.PractitionerRole>(q);
        }

        public Task<R4.Model.Bundle.EntryComponent[]> FindPatientsAsync(SearchParams q)
        {
            return _client.FindResourceBundles<R4.Model.Patient>(q);
        }

        public Task<R4.Model.Bundle.EntryComponent[]> FindObservationsAsync(SearchParams q)
        {
            return _client.FindResourceBundles<R4.Model.Observation>(q);
        }

        public Task<R4.Model.Bundle.EntryComponent[]> FindDiagnosticReportsAsync(SearchParams q)
        {
            return _client.FindResourceBundles<R4.Model.DiagnosticReport>(q);
        }

        public Task<R4.Model.Bundle.EntryComponent[]> FindServiceRequestAsync(SearchParams q)
        {
            return _client.FindResourceBundles<R4.Model.ServiceRequest>(q);
        }

        public Task<R4.Model.Bundle.EntryComponent[]> FindMedicationRequestAsync(SearchParams q)
        {
            return _client.FindResourceBundles<R4.Model.MedicationRequest>(q);
        }
    }
}