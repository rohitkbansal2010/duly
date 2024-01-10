// -----------------------------------------------------------------------
// <copyright file="TestReportsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Annotations.Constants;
using Duly.Common.ApiExtensions.Constants;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
using Duly.Common.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Controllers
{
    /// <summary>
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-246.
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-348.
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-1539.
    /// </summary>
    [Route(RoutePaths.DefaultPatientRoute)]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class TestReportsController : DulyControllerBase
    {
        private const string GetTestReportsForPatientDescription = "Returns an array of TestReport for the specific Patient";
        private const string GetTestReportResultsByReportIdDescription = "Returns detailed information about the test report, including results and the practitioners who performed this report.";

        private readonly ITestReportService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestReportsController" /> class.
        /// </summary>
        /// <param name="service">An instance of Test report service.</param>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public TestReportsController(
            ITestReportService service,
            ILogger<TestReportsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _service = service;
        }

        /// <summary>
        /// Returns all available <see cref="TestReport"/> for the specific Patient.
        /// </summary>
        /// <param name="patientId">Id of Patient.</param>
        /// <param name="startDate">The first for which test reports are required. Format: yyyy-MM-dd.</param>
        /// <param name="endDate">The last for which test reports are required. Format: yyyy-MM-dd.</param>
        /// <param name="amount">The maximum number of test reports to return. Default: 200.</param>
        /// <returns>An array of TestReport for the specific Patient.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(GetTestReportsForPatient),
            Description = GetTestReportsForPatientDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetTestReportsForPatientDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<TestReport>>> GetTestReportsForPatient(
            [Required, FromRoute, SwaggerParameter(RoutePaths.PatientIdDescription)] string patientId,
            [Required, FromQuery, SwaggerParameter("The first for which test reports are required. Format: yyyy-MM-dd."), SwaggerSchema(Format = "date")] DateTime startDate,
            [Required, FromQuery, SwaggerParameter("The last for which test reports are required. Format: yyyy-MM-dd."), SwaggerSchema(Format = "date")] DateTime endDate,
            [FromQuery, SwaggerParameter("The maximum number of test reports to return. Default: 200.")] int amount = 200)
        {
            var interval = new Interval(startDate, endDate);
            if (!IsModelValid(interval, out var errorMsg))
            {
                throw new BadDataException(errorMsg);
            }

            var testReports = await _service.GetTestReportsForPatientAsync(patientId, interval, amount);

            Validate(testReports);

            return Ok(testReports);
        }

        /// <summary>
        /// Returns <see cref="TestReportWithResults"/> that represents detailed information about the test report,
        /// including results and the practitioners who performed this report.
        /// </summary>
        /// <param name="reportId">Id of the test report.</param>
        [HttpGet("/[controller]/reportId")]
        [SwaggerOperation(
            Summary = nameof(GetTestReportResultsByReportId),
            Description = GetTestReportResultsByReportIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetTestReportResultsByReportIdDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<List<MultipleTestReportResults>>> GetTestReportResultsByReportId(
            [Required, SwaggerParameter("Test report Id")] string reportId)
        {
            var reportIdList = reportId.Split(",");
            List<MultipleTestReportResults> results = new List<MultipleTestReportResults>();
            foreach (var id in reportIdList)
            {
                var reportWithResults = await _service.GetTestReportWithResultsByIdAsync(id);
                MultipleTestReportResults result = new MultipleTestReportResults
                {
                    Id = id,
                    TestResultCollection = reportWithResults
                };
                results.Add(result);
            }

            Validate(results);

            return Ok(results);
        }
    }
}
