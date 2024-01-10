// <copyright file="IGetFhirResourceById.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Hl7.Fhir.Model;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Adapters.Interfaces.Composite
{
    public interface IGetFhirResourceById<T>
        where T : Resource
    {
        public Task<T> GetFhirResourceByIdAsync(string id);
    }
}