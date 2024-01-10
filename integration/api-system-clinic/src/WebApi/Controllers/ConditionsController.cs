// -----------------------------------------------------------------------
// <copyright file="ConditionsController.cs" company="Duly Health and Care">
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
    public class ConditionsController : DulyControllerBase
    {
        private const string FindConditionsForPatientDescription = "Returns an array of Conditions for a specific Patient";

        private readonly IConditionRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionsController" /> class.
        /// </summary>
        /// <param name="repository">A repository for working on <see cref="Condition"/>.</param>
        /// <param name="logger">An instance of logger.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public ConditionsController(
            ILogger<ConditionsController> logger,
            IConditionRepository repository,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _repository = repository;
        }

        /// <summary>
        /// Returns all available items of <see cref="Condition"/> for a specific Patient.
        /// </summary>
        /// <param name="patientId">Id of a specific patient.</param>
        /// <param name="status">Clinical status of patients conditions.</param>
        /// <returns>An array of Conditions for a specific Patient.</returns>
        [HttpGet("Problems")]
        [SwaggerOperation(
            Summary = nameof(FindConditionsForPatient),
            Description = FindConditionsForPatientDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, FindConditionsForPatientDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [ObfuscatedIdentityParameters(new[] { "patientId" })]
        public async Task<ActionResult<IEnumerable<Condition>>> FindConditionsForPatient(
            [Required, FromRoute, SwaggerParameter(RoutePaths.PatientIdDescription)] string patientId,
            [Required, FromQuery, SwaggerParameter("Clinical status")] ConditionClinicalStatus[] status)
        {
            var data = await _repository.FindProblemsForPatientAsync(patientId, status);

            Validate(data);

            return Ok(data);
        }
    }
}