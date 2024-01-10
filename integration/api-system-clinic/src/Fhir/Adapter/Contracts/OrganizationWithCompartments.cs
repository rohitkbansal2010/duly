// <copyright file="OrganizationWithCompartments.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Contracts
{
    /// <summary>
    /// Organization and descriptors.
    /// </summary>
    public class OrganizationWithCompartments : IResourceWithCompartments<R4.Organization>
    {
        /// <summary>
        /// Organization.
        /// </summary>
        public R4.Organization Resource { get; set; }
    }
}
