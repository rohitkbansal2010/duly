// <copyright file="ServiceRequestWithCompartments.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Contracts
{
    /// <summary>
    /// Patient and its compartments.
    /// </summary>
    public class ServiceRequestWithCompartments : IResourceWithCompartments<R4.ServiceRequest>
    {
        /// <summary>
        /// Patient.
        /// </summary>
        public R4.ServiceRequest Resource { get; set; }
    }
}