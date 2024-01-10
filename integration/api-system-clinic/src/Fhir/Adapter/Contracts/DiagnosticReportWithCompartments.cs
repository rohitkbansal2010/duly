// <copyright file="DiagnosticReportWithCompartments.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
using r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Contracts
{
    /// <summary>
    /// Diagnostic Report with compartments.
    /// </summary>
    public class DiagnosticReportWithCompartments
    {
        /// <summary>
        /// Diagnostic Report.
        /// </summary>
        public DiagnosticReport Resource { get; set; }

        /// <summary>
        /// Observation from report.
        /// </summary>
        public Observation[] Observations { get; set; }

        /// <summary>
        /// Practitioners who performed report.
        /// </summary>
        public PractitionerWithRoles[] Performers { get; set; }
    }
}