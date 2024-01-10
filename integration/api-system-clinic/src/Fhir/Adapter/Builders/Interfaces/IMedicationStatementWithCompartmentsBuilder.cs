// <copyright file="IMedicationStatementWithCompartmentsBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias stu3;

using Duly.Clinic.Fhir.Adapter.Contracts;
using System.Collections.Generic;

using STU3 = stu3::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Builders.Interfaces
{
    /// <summary>
    /// Can build MedicationStatementWithCompartments.
    /// </summary>
    public interface IMedicationStatementWithCompartmentsBuilder
    {
        /// <summary>
        /// Gets Medications from Bundles.
        /// </summary>
        /// <param name="searchResult">Collection of search results.</param>
        /// <param name="practitionersWithRoles">Practitioners with their roles.</param>
        /// <returns>Complete resources.</returns>
        MedicationStatementWithCompartments[] ExtractMedicationWithCompartments(
            IEnumerable<STU3.Bundle.EntryComponent> searchResult,
            PractitionerWithRolesSTU3[] practitionersWithRoles);
    }
}
