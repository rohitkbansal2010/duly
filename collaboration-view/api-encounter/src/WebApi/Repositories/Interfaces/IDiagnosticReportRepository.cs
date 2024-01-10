// <copyright file="IDiagnosticReportRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Models.DiagnosticReport"/> model.
    /// </summary>
    internal interface IDiagnosticReportRepository
    {
        /// <summary>
        /// Retrieves all items of <see cref="Models.DiagnosticReport"/> that match the filtering parameters.
        /// </summary>
        /// <param name="patientId">Identifier of a specific patient.</param>
        /// <param name="startPeriod">Start of the period.</param>
        /// <param name="endPeriod">End of the period.</param>
        /// <returns>Filtered items of <see cref="Models.DiagnosticReport"/>.</returns>
        Task<IEnumerable<Models.DiagnosticReport>> GetDiagnosticReportsForPatientAsync(
            string patientId,
            DateTimeOffset startPeriod,
            DateTimeOffset endPeriod);

        /// <summary>
        /// Retrieves <see cref="Models.DiagnosticReport"/> by the report Id.
        /// </summary>
        /// <param name="reportId">Id of the diagnostic report.</param>
        /// <returns>An instance of <see cref="Models.DiagnosticReport"/>.</returns>
        Task<Models.DiagnosticReport> GetDiagnosticReportByIdAsync(string reportId);
    }
}
