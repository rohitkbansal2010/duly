// <copyright file="TestReportStatusConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    internal class TestReportStatusConverter : ITypeConverter<DiagnosticReportStatus, TestReportStatus>
    {
        public TestReportStatus Convert(DiagnosticReportStatus source, TestReportStatus destination, ResolutionContext context)
        {
            return source switch
            {
                DiagnosticReportStatus.Amended => TestReportStatus.Amended,
                DiagnosticReportStatus.Appended => TestReportStatus.Appended,
                DiagnosticReportStatus.Cancelled => TestReportStatus.Cancelled,
                DiagnosticReportStatus.Corrected => TestReportStatus.Corrected,
                DiagnosticReportStatus.Final => TestReportStatus.Final,
                DiagnosticReportStatus.Partial => TestReportStatus.Partial,
                DiagnosticReportStatus.Preliminary => TestReportStatus.Preliminary,
                DiagnosticReportStatus.Registered => TestReportStatus.Registered,
                DiagnosticReportStatus.Unknown => TestReportStatus.Unknown,
                _ => throw new ServiceNotMappedException("Could not map DiagnosticReportStatus to TestReportStatus")
            };
        }
    }
}