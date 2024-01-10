// -----------------------------------------------------------------------
// <copyright file="TestReport.Status.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Statusses of test report")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1602:Enumeration items should be documented",
        Justification = "Contract")]
    public enum TestReportStatus
    {
        [Description("The existence of the report is registered, but there is nothing yet available.")]
        Registered,

        [Description("This is a partial (e.g. initial, interim or preliminary) report: data in the report may be incomplete or unverified.")]
        Partial,

        [Description("Verified early results are available, but not all results are final.")]
        Preliminary,

        [Description("The report is complete and verified by an authorized person.")]
        Final,

        [Description("Subsequent to being final, the report has been modified. This includes any change in the results, diagnosis, narrative text, or other content of a report that has been issued.")]
        Amended,

        [Description("Subsequent to being final, the report has been modified to correct an error in the report or referenced results.")]
        Corrected,

        [Description("Subsequent to being final, the report has been modified by adding new content. The existing content is unchanged.")]
        Appended,

        [Description("The report is unavailable because the measurement was not started or not completed (also sometimes called \"aborted\").")]
        Cancelled,

        [Description("The authoring/source system does not know which of the status values currently applies for this observation.")]
        Unknown
    }
}