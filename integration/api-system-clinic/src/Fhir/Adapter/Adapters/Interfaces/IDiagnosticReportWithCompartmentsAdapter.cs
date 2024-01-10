// <copyright file="IDiagnosticReportWithCompartmentsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// Adapter to work on DiagnosticReports with compartments.
    /// </summary>
    public interface IDiagnosticReportWithCompartmentsAdapter
    {
        /// <summary>
        /// Gets DiagnosticReports and their compartments.
        /// </summary>
        /// <param name="searchCriteria">Search criteria from list of parameters.</param>
        /// <returns>All data that satisfies search criteria.</returns>
        Task<IEnumerable<DiagnosticReportWithCompartments>> FindDiagnosticReportsWithCompartmentsAsync(
            DiagnosticReportSearchCriteria searchCriteria);

        /// <summary>
        /// Gets DiagnosticReport and its compartments.
        /// </summary>
        /// <param name="reportId">Identifier of the report.</param>
        /// <returns><see cref="DiagnosticReportWithCompartments"/> item with compartments.</returns>
        Task<DiagnosticReportWithCompartments> FindDiagnosticReportWithCompartmentsByIdAsync(string reportId);
    }
}
