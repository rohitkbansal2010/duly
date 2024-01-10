// <copyright file="FhirClientWrapperStu3.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias stu3;

using Duly.Clinic.Fhir.Adapter.Extensions.Fhir;
using Hl7.Fhir.Rest;
using System.Threading.Tasks;

using STU3 = stu3::Hl7.Fhir;

namespace Duly.Clinic.Fhir.Adapter.Extensions
{
    internal class FhirClientWrapperStu3 : IFhirClientSTU3
    {
        private readonly STU3.Rest.FhirClient _client;

        public FhirClientWrapperStu3(STU3.Rest.FhirClient client)
        {
            _client = client;
        }

        public Task<STU3.Model.Bundle.EntryComponent[]> FindMedicationStatementsAsync(SearchParams q)
        {
            return _client.FindResourceBundles<STU3.Model.MedicationStatement>(q);
        }

        public Task<STU3.Model.Bundle.EntryComponent[]> FindPractitionerRolesAsync(SearchParams q)
        {
            return _client.FindResourceBundles<STU3.Model.PractitionerRole>(q);
        }

        public Task<STU3.Model.Bundle.EntryComponent[]> FindEncountersAsync(SearchParams q)
        {
            return _client.FindResourceBundles<STU3.Model.Encounter>(q);
        }
    }
}