// <copyright file="ImmunizationsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

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
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-1286.
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-1096.
    /// </summary>
    [Route(RoutePaths.DefaultPatientRoute)]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class ImmunizationsController : DulyControllerBase
    {
        private const string GetImmunizationByPatientIdDescription = "Returns an information about a patient's immunization, including information on recommended and past immunizations, as well as the patient's immunization progress.";

        private readonly IImmunizationService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmunizationsController" /> class.
        /// </summary>
        /// <param name="service">An instance of ImmunizationService service.</param>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public ImmunizationsController(
            IImmunizationService service,
            ILogger<ImmunizationsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _service = service;
        }

        /// <summary>
        /// Returns <see cref="Immunizations"/> that represents an information about a patient's immunization,
        /// including information on recommended and past immunizations, as well as the patient's immunization progress.
        /// </summary>
        /// <param name="patientId">Id of a specific patient.</param>
        /// <returns>An instance of <see cref="Immunizations"/> for a specific patient.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(GetImmunizationByPatientId),
            Description = GetImmunizationByPatientIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetImmunizationByPatientIdDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<Immunizations>> GetImmunizationByPatientId(
            [Required, FromRoute, SwaggerParameter(RoutePaths.PatientIdDescription)] string patientId)
        {
            var immunization = await _service.GetImmunizationsByPatientIdAsync(patientId);

            Validate(immunization);

            return Ok(immunization);
        }
    }
}
