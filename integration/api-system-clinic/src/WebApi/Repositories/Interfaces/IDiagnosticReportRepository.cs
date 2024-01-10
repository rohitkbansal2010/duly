// <copyright file="IDiagnosticReportRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="DiagnosticReport"/> entities.
    /// </summary>
    public interface IDiagnosticReportRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="DiagnosticReport"/> which match with the filter.
        /// </summary>
        /// <param name="patientId">Identifier of Patient.</param>
        /// <param name="startPeriod">Start of the period.</param>
        /// <param name="endPeriod">End of the period.</param>
        /// <returns>Filtered items of <see cref="DiagnosticReport"/>.</returns>
        Task<IEnumerable<DiagnosticReport>> FindDiagnosticReportsForPatientAsync(string patientId, DateTimeOffset startPeriod, DateTimeOffset endPeriod);

        /// <summary>
        /// Retrieve a <see cref="DiagnosticReport"/> with details by specific Id.
        /// </summary>
        /// <param name="reportId">Identifier of DiagnosticReport.</param>
        /// <returns><see cref="DiagnosticReport"/> with details.</returns>
        Task<DiagnosticReport> FindDiagnosticReportByIdAsync(string reportId);
    }
}