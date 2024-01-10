// <copyright file="IDiagnosticReportWithCompartmentsBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using Duly.Clinic.Fhir.Adapter.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Builders.Interfaces
{
    internal interface IDiagnosticReportWithCompartmentsBuilder
    {
        /// <summary>
        /// Extract DiagnosticReport with compartments from Bundles.
        /// </summary>
        /// <param name="searchResult">Entries from search.</param>
        /// <returns>Extracted reports, practitioners and observations.</returns>
        Task<IEnumerable<DiagnosticReportWithCompartments>> ExtractDiagnosticReportWithCompartmentsAsync(IEnumerable<R4.Bundle.EntryComponent> searchResult);

        /// <summary>
        /// Extract DiagnosticReport with compartments from Bundles.
        /// </summary>
        /// <param name="searchResult">Entries from search.</param>
        /// <returns>Extracted DiagnosticReport.</returns>
        Task<DiagnosticReportWithCompartments> ExtractDiagnosticReportWithCompartmentsByIdAsync(IEnumerable<R4.Bundle.EntryComponent> searchResult);
    }
}