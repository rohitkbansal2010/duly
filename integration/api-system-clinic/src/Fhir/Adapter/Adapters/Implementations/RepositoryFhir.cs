// <copyright file="RepositoryFhir.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces.Composite;
using Duly.Clinic.Fhir.Adapter.Extensions;
using Hl7.Fhir.Model;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Adapters.Implementations
{
    public class RepositoryFhir<TIn> : IGetFhirResource<TIn>
        where TIn : Resource, new()
    {
        private readonly IFhirClientR4 _client;

        public RepositoryFhir(IFhirClientR4 client)
        {
            _client = client;
        }

        #region IGetFhirResource

        public async Task<TIn> GetFhirResourceByIdAsync(string id)
        {
            var bundle = await _client.SearchByIdAsync<TIn>(id);
            if (bundle.Entry.Count == 0)
            {
                return null;
            }

            return (TIn)bundle.Entry[0].Resource;
        }

        #endregion
    }
}
