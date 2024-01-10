// <copyright file="TestReportService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="ITestReportService"/>
    /// </summary>
    internal class TestReportService : ITestReportService
    {
        private static readonly DiagnosticReportStatus[] Statuses =
        {
            DiagnosticReportStatus.Registered,
            DiagnosticReportStatus.Partial,
            DiagnosticReportStatus.Amended,
            DiagnosticReportStatus.Appended,
            DiagnosticReportStatus.Corrected,
            DiagnosticReportStatus.Cancelled,
            DiagnosticReportStatus.Unknown,
            DiagnosticReportStatus.Preliminary,
            DiagnosticReportStatus.Final
        };

        private readonly IDiagnosticReportRepository _repository;
        private readonly IMapper _mapper;

        public TestReportService(
            IMapper mapper,
            IDiagnosticReportRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<TestReport>> GetTestReportsForPatientAsync(
            string patientId,
            Interval interval,
            int amount)
        {
            var modelTestReports = await _repository.GetDiagnosticReportsForPatientAsync(
                patientId,
                new(interval.StartDate, TimeSpan.Zero),
                new(interval.EndDate.AddDays(1).AddMilliseconds(-1), TimeSpan.Zero));
            var modelTestReportsFiltered = modelTestReports.Where(x =>
                Statuses.Contains(x.Status));

            var testReports = _mapper.Map<IEnumerable<TestReport>>(modelTestReportsFiltered);

            return testReports
                .OrderByDescending(x => x.Date)
                .ThenBy(x => x.Title)
                .Take(amount);
        }

        public async Task<TestReportWithResults> GetTestReportWithResultsByIdAsync(string reportId)
        {
            var diagnosticReport = await _repository.GetDiagnosticReportByIdAsync(reportId);
            if (diagnosticReport == null || !Statuses.Contains(diagnosticReport.Status))
            {
                return null;
            }

            return _mapper.Map<TestReportWithResults>(diagnosticReport);
        }
    }
}
