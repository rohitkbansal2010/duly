// <copyright file="IPatientWithCompartmentsBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using Duly.Clinic.Fhir.Adapter.Contracts;
using System.Collections.Generic;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Builders.Interfaces
{
    /// <summary>
    /// Can build CareTeamWithCompartmentsBuilder.
    /// </summary>
    public interface IPatientWithCompartmentsBuilder
    {
        /// <summary>
        /// Extract patient with compartments from Bundles.
        /// </summary>
        /// <param name="searchResult">Entries from search.</param>
        /// <returns>Extracted care teams with participants.</returns>
        PatientWithCompartments ExtractPatientWithCompartments(IEnumerable<R4.Bundle.EntryComponent> searchResult);

        /// <summary>
        /// Extract patients with compartments from Bundles.
        /// </summary>
        /// <param name="searchResult">Entries from search.</param>
        /// <returns>Extracted list of patients with participants.</returns>
        PatientWithCompartments[] ExtractPatientsWithCompartments(IEnumerable<R4.Bundle.EntryComponent> searchResult);
    }
}