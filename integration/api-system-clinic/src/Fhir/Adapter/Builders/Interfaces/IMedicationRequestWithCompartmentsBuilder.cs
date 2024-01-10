// <copyright file="IMedicationRequestWithCompartmentsBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
using Duly.Clinic.Fhir.Adapter.Contracts;
using System.Collections.Generic;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Builders.Interfaces
{
    /// <summary>
    /// Can build MedicationRequestWithCompartmentsBuilder.
    /// </summary>
    public interface IMedicationRequestWithCompartmentsBuilder
    {
        /// <summary>
        /// Gets Medications from Bundles.
        /// </summary>
        /// <param name="searchResult">Collection of search results.</param>
        /// <param name="practitionersWithRoles">Practitioners with their roles.</param>
        /// <returns>Complete resources.</returns>
        MedicationRequestWithCompartments[] ExtractMedicationRequestWithCompartments(
            IEnumerable<R4.Bundle.EntryComponent> searchResult,
            PractitionerWithRoles[] practitionersWithRoles);
    }
}
