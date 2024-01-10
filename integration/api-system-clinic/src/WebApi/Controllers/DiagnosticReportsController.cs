// -----------------------------------------------------------------------
// <copyright file="DiagnosticReportsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Common.Annotations.Constants;
using Duly.Common.ApiExtensions.Constants;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
using Duly.Common.Security.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Controllers
{
    [Route(RoutePaths.DefaultPatientRoute)]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    public class DiagnosticReportsController : DulyControllerBase
    {
        private const string FindDiagnosticReportsForPatientDescription = "Returns an array of DiagnosticReport for a specific Patient";
        private const string FindDiagnosticReportByIdDescription = "Returns a DiagnosticReport by a report Id";

        private readonly IDiagnosticReportRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticReportsController" /> class.
        /// </summary>
        /// <param name="repository">A repository for working on <see cref="DiagnosticReport"/>.</param>
        /// <param name="logger">An instance of logger exampleProvider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public DiagnosticReportsController(
            IDiagnosticReportRepository repository,
            ILogger<DiagnosticReportsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _repository = repository;
        }

        /// <summary>
        /// Returns available items of <see cref="DiagnosticReport"/> for a specific Patient in specified range.
        /// </summary>
        /// <param name="patientId">Id of a specific patient.</param>
        /// <param name="startPeriod">Start of the period.</param>
        /// <param name="endPeriod">End of the period.</param>
        /// <returns>An array of <see cref="DiagnosticReport"/> for a specific Patient.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(FindDiagnosticReportsForPatient),
            Description = FindDiagnosticReportsForPatientDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, FindDiagnosticReportsForPatientDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [ObfuscatedIdentityParameters(new[] { "patientId" })]
        public async Task<ActionResult<IEnumerable<DiagnosticReport>>> FindDiagnosticReportsForPatient(
            [Required, FromRoute, SwaggerParameter(RoutePaths.PatientIdDescription)] string patientId,
            [Required, FromQuery, SwaggerParameter("Start period")] DateTimeOffset startPeriod,
            [Required, FromQuery, SwaggerParameter("End period")] DateTimeOffset endPeriod)
        {
            var data = await _repository.FindDiagnosticReportsForPatientAsync(patientId, startPeriod, endPeriod);

            Validate(data);

            return Ok(data);
        }

        /// <summary>
        /// Returns <see cref="DiagnosticReport"/> by a report Id.
        /// </summary>
        /// <param name="reportId">Id of the diagnostic report.</param>
        /// <returns>An instance of <see cref="DiagnosticReport"/>.</returns>
        [HttpGet("/[controller]/{reportId}")]
        [SwaggerOperation(
            Summary = nameof(FindDiagnosticReportById),
            Description = FindDiagnosticReportByIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, FindDiagnosticReportByIdDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [ObfuscatedIdentityParameters(new[] { "reportId" })]
        public async Task<ActionResult<DiagnosticReport>> FindDiagnosticReportById(
            [Required, FromRoute, SwaggerParameter("Diagnostic report Id")] string reportId)
        {
            var data = await _repository.FindDiagnosticReportByIdAsync(reportId);

            Validate(data);

            return Ok(data);
        }
    }
}