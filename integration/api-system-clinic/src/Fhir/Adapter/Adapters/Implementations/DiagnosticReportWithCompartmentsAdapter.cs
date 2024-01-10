// <copyright file="DiagnosticReportWithCompartmentsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Extensions;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Adapters.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IDiagnosticReportWithCompartmentsAdapter"/>
    /// </summary>
    internal class DiagnosticReportWithCompartmentsAdapter : IDiagnosticReportWithCompartmentsAdapter
    {
        /// <summary>
        /// Compartments which can be retrieved with the main resource.
        /// </summary>
        private static readonly string[] DiagnosticReportWithCompartmentsAdapterIncludes = { "DiagnosticReport:result:Observation", "DiagnosticReport:performer:Practitioner" };

        private readonly IFhirClientR4 _client;
        private readonly IDiagnosticReportWithCompartmentsBuilder _builder;

        public DiagnosticReportWithCompartmentsAdapter(IFhirClientR4 client, IDiagnosticReportWithCompartmentsBuilder builder)
        {
            _client = client;
            _builder = builder;
        }

        public async Task<IEnumerable<DiagnosticReportWithCompartments>> FindDiagnosticReportsWithCompartmentsAsync(DiagnosticReportSearchCriteria searchCriteria)
        {
            var searchResult = await _client.FindDiagnosticReportsAsync(searchCriteria.ToSearchParams().AddIncludes(DiagnosticReportWithCompartmentsAdapterIncludes));

            return await _builder.ExtractDiagnosticReportWithCompartmentsAsync(searchResult);
        }

        public async Task<DiagnosticReportWithCompartments> FindDiagnosticReportWithCompartmentsByIdAsync(string reportId)
        {
            var searchParams = new SearchParams().ById(reportId).AddIncludes(DiagnosticReportWithCompartmentsAdapterIncludes);
            var searchResult = await _client.FindDiagnosticReportsAsync(searchParams);
            if (searchResult.IsNullOrEmpty())
            {
                return null;
            }

            return await _builder.ExtractDiagnosticReportWithCompartmentsByIdAsync(searchResult);
        }
    }
}
