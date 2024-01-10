// <copyright file="IRelatedPersonWithCompartmentsBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using Duly.Clinic.Fhir.Adapter.Contracts;
using System.Collections.Generic;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Builders.Interfaces
{
    /// <summary>
    /// Can build RelatedPersonWithCompartments.
    /// </summary>
    internal interface IRelatedPersonWithCompartmentsBuilder
    {
        /// <summary>
        /// Extract related persons with compartments from Bundles.
        /// </summary>
        /// <param name="searchResult">Entries from search.</param>
        /// <returns>Extracted related persons.</returns>
        RelatedPersonWithCompartments[] ExtractRelatedPersons(IEnumerable<R4.Bundle.EntryComponent> searchResult);
    }
}
