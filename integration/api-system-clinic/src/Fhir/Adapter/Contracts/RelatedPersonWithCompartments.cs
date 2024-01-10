// <copyright file="RelatedPersonWithCompartments.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Contracts
{
    /// <summary>
    /// Related person and descriptors.
    /// </summary>
    public class RelatedPersonWithCompartments : IResourceWithCompartments<R4.RelatedPerson>
    {
        /// <summary>
        /// Related person.
        /// </summary>
        public R4.RelatedPerson Resource { get; set; }
    }
}