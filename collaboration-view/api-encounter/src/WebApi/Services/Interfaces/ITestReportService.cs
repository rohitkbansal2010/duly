// <copyright file="ITestReportService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.TestReport"/> entity.
    /// </summary>
    public interface ITestReportService
    {
        /// <summary>
        /// Retrieve <see cref="Contracts.TestReport"/> items that match with the filtering parameter.
        /// </summary>
        /// <param name="patientId">The identifier of a specific patient.</param>
        /// <param name="interval">The first for which test reports are required. Format: yyyy-MM-dd.</param>
        /// <param name="amount">The maximum number of test reports to return.</param>
        /// <returns>An instance of <see cref="Contracts.TestReport"/> for a specific patient.</returns>
        Task<IEnumerable<TestReport>> GetTestReportsForPatientAsync(
            string patientId,
            Interval interval,
            int amount);

        /// <summary>
        /// Retrieves <see cref="TestReportWithResults"/> by the report Id.
        /// </summary>
        /// <param name="reportId">Id of the test report.</param>
        /// <returns>An instance of <see cref="Contracts.TestReportWithResults"/>.</returns>
        Task<Contracts.TestReportWithResults> GetTestReportWithResultsByIdAsync(string reportId);
    }
}
