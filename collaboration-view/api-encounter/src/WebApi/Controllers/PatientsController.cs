// -----------------------------------------------------------------------
// <copyright file="PatientsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Annotations.Constants;
using Duly.Common.ApiExtensions.Constants;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Controllers
{
    /// <summary>
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-315.
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-339.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class PatientsController : DulyControllerBase
    {
        private const string DescriptionGetPatient = "Returns information about a specific Patient";

        private readonly IPatientService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="service">An instance of Patient service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public PatientsController(
            IPatientService service,
            ILogger<PatientsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _service = service;
        }

        /// <summary>
        /// Returns <see cref="Patient"/> for a specific patient.
        /// </summary>
        /// <param name="patientId">Id of a specific patient.</param>
        /// <returns>Retrieved <see cref="Patient"/> instance.</returns>
        [HttpGet(RoutePaths.PatientIdName)]
        [SwaggerOperation(
            Summary = nameof(GetPatientById),
            Description = DescriptionGetPatient)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionGetPatient)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<Patient>> GetPatientById(
            [Required, FromRoute, SwaggerParameter(RoutePaths.PatientIdDescription)] string patientId)
        {
            var patient = await _service.GetPatientByIdAsync(patientId);

            Validate(patient);
            return Ok(patient);
        }
    }
}