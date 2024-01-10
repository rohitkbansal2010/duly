// <copyright file="FhirDiagnosticReportRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Common.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IDiagnosticReportRepository"/>
    /// </summary>
    internal class FhirDiagnosticReportRepository : IDiagnosticReportRepository
    {
        private readonly IDiagnosticReportWithCompartmentsAdapter _adapter;
        private readonly IMapper _mapper;

        public FhirDiagnosticReportRepository(IDiagnosticReportWithCompartmentsAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DiagnosticReport>> FindDiagnosticReportsForPatientAsync(string patientId, DateTimeOffset startPeriod, DateTimeOffset endPeriod)
        {
            var searchCriteria = BuildSearchCriteria(patientId, startPeriod, endPeriod);
            var fhirReports = await _adapter.FindDiagnosticReportsWithCompartmentsAsync(searchCriteria);
            var systemReports = _mapper.Map<IEnumerable<DiagnosticReport>>(fhirReports);
            return systemReports.Where(x => x.Status != DiagnosticReportStatus.EnteredInError);
        }

        public async Task<DiagnosticReport> FindDiagnosticReportByIdAsync(string reportId)
        {
            var fhirReport = await _adapter.FindDiagnosticReportWithCompartmentsByIdAsync(reportId);
            if (fhirReport == null)
            {
                throw new EntityNotFoundException(nameof(DiagnosticReport), reportId);
            }

            return _mapper.Map<DiagnosticReport>(fhirReport);
        }

        private static DiagnosticReportSearchCriteria BuildSearchCriteria(string patientId, DateTimeOffset startPeriod, DateTimeOffset endPeriod)
        {
            //Maybe search criteria should be changed to DateTime
            var searchCriteria = new DiagnosticReportSearchCriteria { PatientId = patientId, StartDateTime = startPeriod.UtcDateTime, EndDateTime = endPeriod.UtcDateTime };
            return searchCriteria;
        }
    }
}
