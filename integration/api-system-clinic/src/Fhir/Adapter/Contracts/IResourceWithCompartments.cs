// <copyright file="IResourceWithCompartments.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Contracts
{
    /// <summary>
    /// Resource that has been joined with its compartments.
    /// </summary>
    /// <typeparam name="T">Primary resource.</typeparam>
    public interface IResourceWithCompartments<T>
        where T : Resource
    {
        /// <summary>
        /// Primary resource.
        /// </summary>
        T Resource { get; set; }
    }
}
