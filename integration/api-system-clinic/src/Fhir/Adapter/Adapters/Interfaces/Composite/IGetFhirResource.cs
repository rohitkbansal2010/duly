// <copyright file="IGetFhirResource.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Adapters.Interfaces.Composite
{
    public interface IGetFhirResource<T> : IGetFhirResourceById<T>
        where T : Resource
    {
    }
}