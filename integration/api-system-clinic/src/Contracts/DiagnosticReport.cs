// <copyright file="DiagnosticReport.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Security.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    public class DiagnosticReport
    {
        [Description("DiagnosticReport Id")]
        [Required]
        [Identity]
        public string Id { get; set; }

        [Description("Name of a Diagnostic Report")]
        public string Name { get; set; }

        [Description("Effective Date of Diagnostic Report")]
        public DateTimeOffset? EffectiveDate { get; set; }

        [Description("Date when Diagnostic Report was issued")]
        public DateTimeOffset? Issued { get; set; }

        [Description("Status of DiagnosticReport")]
        [Required]
        public DiagnosticReportStatus Status { get; set; }

        [Description("Practitioner who performed this report")]
        public PractitionerGeneralInfo[] Performers { get; set; }

        [Description("Observations from the report")]
        public ObservationLabResult[] Observations { get; set; }
    }
}