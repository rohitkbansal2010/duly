// <copyright file="DiagnosticReportConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
using AutoMapper;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts;
using System;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps DiagnosticReportWithCompartments into DiagnosticReport.
    /// </summary>
    public class DiagnosticReportConverter : ITypeConverter<DiagnosticReportWithCompartments, DiagnosticReport>
    {
        public DiagnosticReport Convert(DiagnosticReportWithCompartments source, DiagnosticReport destination, ResolutionContext context)
        {
            return new DiagnosticReport
            {
                Id = source.Resource.Id,
                EffectiveDate = context.Mapper.Map<DateTimeOffset>(source.Resource.Effective),
                Issued = source.Resource.Issued,
                Name = source.Resource.Code.Text,
                Observations = context.Mapper.Map<ObservationLabResult[]>(source.Observations),
                Status = MapStatus(source.Resource.Status),
                Performers = context.Mapper.Map<PractitionerGeneralInfo[]>(source.Performers)
            };
        }

        private static DiagnosticReportStatus MapStatus(R4.DiagnosticReport.DiagnosticReportStatus? status)
        {
            return status switch
            {
                R4.DiagnosticReport.DiagnosticReportStatus.Final => DiagnosticReportStatus.Final,
                R4.DiagnosticReport.DiagnosticReportStatus.Amended => DiagnosticReportStatus.Amended,
                R4.DiagnosticReport.DiagnosticReportStatus.Appended => DiagnosticReportStatus.Appended,
                R4.DiagnosticReport.DiagnosticReportStatus.Cancelled => DiagnosticReportStatus.Cancelled,
                R4.DiagnosticReport.DiagnosticReportStatus.Corrected => DiagnosticReportStatus.Corrected,
                R4.DiagnosticReport.DiagnosticReportStatus.EnteredInError => DiagnosticReportStatus.EnteredInError,
                R4.DiagnosticReport.DiagnosticReportStatus.Partial => DiagnosticReportStatus.Partial,
                R4.DiagnosticReport.DiagnosticReportStatus.Preliminary => DiagnosticReportStatus.Preliminary,
                R4.DiagnosticReport.DiagnosticReportStatus.Registered => DiagnosticReportStatus.Registered,
                R4.DiagnosticReport.DiagnosticReportStatus.Unknown => DiagnosticReportStatus.Unknown,
                _ => throw new ConceptNotMappedException("DiagnosticReportStatus not mapped")
            };
        }
    }
}