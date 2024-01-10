// <copyright file="DiagnosticReport.Status.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Statuses of diagnostic report.
    /// </summary>
    internal enum DiagnosticReportStatus
    {
        /// <summary>
        /// The existence of the report is registered, but there is nothing yet available.
        /// </summary>
        Registered,

        /// <summary>
        /// This is a partial (e.g. initial, interim or preliminary) report: data in the report may be incomplete or unverified.
        /// </summary>
        Partial,

        /// <summary>
        /// Verified early results are available, but not all results are final.
        /// </summary>
        Preliminary,

        /// <summary>
        /// The report is complete and verified by an authorized person.
        /// </summary>
        Final,

        /// <summary>
        /// Subsequent to being final, the report has been modified. This includes any change in the results, diagnosis, narrative text, or other content of a report that has been issued.
        /// </summary>
        Amended,

        /// <summary>
        /// Subsequent to being final, the report has been modified to correct an error in the report or referenced results.
        /// </summary>
        Corrected,

        /// <summary>
        /// Subsequent to being final, the report has been modified by adding new content. The existing content is unchanged.
        /// </summary>
        Appended,

        /// <summary>
        /// The report is unavailable because the measurement was not started or not completed (also sometimes called "aborted").
        /// </summary>
        Cancelled,

        /// <summary>
        /// The report has been withdrawn following a previous final release.
        /// </summary>
        EnteredInError,

        /// <summary>
        /// The authoring/source system does not know which of the status values currently applies for this observation.
        /// </summary>
        Unknown
    }
}
