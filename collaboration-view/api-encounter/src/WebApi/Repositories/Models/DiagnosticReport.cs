// <copyright file="DiagnosticReport.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Diagnostic report for a specific patient.
    /// </summary>
    internal class DiagnosticReport
    {
        /// <summary>
        /// DiagnosticReport Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of a Diagnostic Report.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Effective Date of Diagnostic Report.
        /// </summary>
        public DateTimeOffset? EffectiveDate { get; set; }

        /// <summary>
        /// Date when Diagnostic Report was issued.
        /// </summary>
        public DateTimeOffset? Issued { get; set; }

        /// <summary>
        /// Status of DiagnosticReport.
        /// </summary>
        public DiagnosticReportStatus Status { get; set; }

        /// <summary>
        /// Practitioner who performed this report.
        /// </summary>
        public PractitionerGeneralInfo[] Performers { get; set; }

        /// <summary>
        /// Observations from the report.
        /// </summary>
        public ObservationLabResult[] Observations { get; set; }
    }
}
