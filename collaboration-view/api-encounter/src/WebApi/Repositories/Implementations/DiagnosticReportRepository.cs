// -----------------------------------------------------------------------
// <copyright file="DiagnosticReportRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using AutoMapper;
using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IDiagnosticReportRepository"/>
    /// </summary>
    internal class DiagnosticReportRepository : IDiagnosticReportRepository
    {
        private readonly IEncounterContext _encounterContext;
        private readonly IMapper _mapper;
        private readonly IPatientsClient _patientsClient;
        private readonly IClient _commonClient;

        public DiagnosticReportRepository(
            IEncounterContext encounterContext,
            IMapper mapper,
            IPatientsClient patientsClient,
            IClient commonClient)
        {
            _encounterContext = encounterContext;
            _mapper = mapper;
            _patientsClient = patientsClient;
            _commonClient = commonClient;
        }

        public async Task<Models.DiagnosticReport> GetDiagnosticReportByIdAsync(string reportId)
        {
            var report = await _commonClient.DiagnosticReportsAsync(reportId, _encounterContext.GetXCorrelationId());

            return _mapper.Map<Models.DiagnosticReport>(report);
        }

        public async Task<IEnumerable<Models.DiagnosticReport>> GetDiagnosticReportsForPatientAsync(
            string patientId,
            DateTimeOffset startPeriod,
            DateTimeOffset endPeriod)
        {
            var reports = await _patientsClient.DiagnosticReportsAsync(
                patientId,
                startPeriod,
                endPeriod,
                _encounterContext.GetXCorrelationId());

            return _mapper.Map<IEnumerable<Models.DiagnosticReport>>(reports);
        }
    }
}
