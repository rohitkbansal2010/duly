// <copyright file="HealthConditionsController.cs" company="Duly Health and Care">
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
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-380.
    /// </summary>
    [Route(RoutePaths.DefaultPatientRoute)]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class HealthConditionsController : DulyControllerBase
    {
        private const string GetHealthConditionsByPatientIdDescription = "Returns an information about all of a patient's problems in two sets of health conditions: Current health conditions and Previous health conditions.";

        private readonly IConditionService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthConditionsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="service">An instance of Condition service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public HealthConditionsController(
            IConditionService service,
            ILogger<HealthConditionsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _service = service;
        }

        /// <summary>
        /// Returns <see cref="HealthConditions"/> that represents an information about all of a patient's problems
        /// in two sets of health conditions: Current health conditions and previous Health conditions.
        /// </summary>
        /// <param name="patientId">Id of a specific patient.</param>
        /// <returns>An instance of <see cref="HealthConditions"/> for a specific patient.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(GetHealthConditionsByPatientId),
            Description = GetHealthConditionsByPatientIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetHealthConditionsByPatientIdDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<HealthConditions>> GetHealthConditionsByPatientId(
            [Required, FromRoute, SwaggerParameter(RoutePaths.PatientIdDescription)] string patientId)
        {
            var healthConditions = await _service.GetHealthConditionsByPatientIdAsync(patientId);

            Validate(healthConditions);

            return Ok(healthConditions);
        }
    }
}
