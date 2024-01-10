// <copyright file="CareTeamController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

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
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    public class CareTeamController : DulyControllerBase
    {
        private const string GetCareTeamParticipantsByEncounterIdDescription = "Returns a list of Care Team participants involved in the encounter";
        private const string GetCareTeamParticipantsByPatientIdDescription = "Returns list of Care Team members linked to patient.";

        private readonly ICareTeamParticipantRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CareTeamController" /> class.
        /// </summary>
        /// <param name="repository">A repository for working on <see cref="CareTeamParticipant"/>.</param>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public CareTeamController(
            ICareTeamParticipantRepository repository,
            ILogger<CareTeamController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _repository = repository;
        }

        /// <summary>
        /// Returns list of <see cref="CareTeamParticipant"/> involved in the encounter.
        /// </summary>
        /// <param name="encounterId">Id of a specific encounter.</param>
        /// <param name="status">Status of required Care teams.</param>
        /// <param name="category">Category of Care teams.</param>
        /// <returns>Returns list of Care Team members involved in the encounter.</returns>
        [Route(RoutePaths.DefaultEncounterRoute + "/participants")]
        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(GetCareTeamParticipantsByEncounterId),
            Description = GetCareTeamParticipantsByEncounterIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, "List of Care Team participants involved in the encounter.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [ObfuscatedIdentityParameters(new[] { "encounterId" })]
        public async Task<ActionResult<IEnumerable<CareTeamParticipant>>> GetCareTeamParticipantsByEncounterId(
            [Required, FromRoute, SwaggerParameter("Encounter Id")] string encounterId,
            [Required, FromQuery, SwaggerParameter("Care Team Status")] CareTeamStatus status,
            [Required, FromQuery, SwaggerParameter("Care Team Category")] CareTeamCategory category)
        {
            var data = await _repository.GetCareTeamsParticipantsByEncounterIdAsync(encounterId, status, category);

            Validate(data);

            return Ok(data);
        }

        /// <summary>
        /// Returns list of <see cref="CareTeamParticipant"/> linked to patient.
        /// </summary>
        /// <param name="patientId">Id of a patient.</param>
        /// <param name="status">Status of required Care teams.</param>
        /// <param name="category">Category of Care teams.</param>
        /// <returns>Returns list of Care Team members linked to patient.</returns>
        [Route(RoutePaths.DefaultPatientRoute + "/members")]
        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(GetCareTeamParticipantsByPatientId),
            Description = GetCareTeamParticipantsByPatientIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, "List of Care Team members linked to patient.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [ObfuscatedIdentityParameters(new[] { "patientId" })]
        public async Task<ActionResult<IEnumerable<CareTeamParticipant>>> GetCareTeamParticipantsByPatientId(
            [Required, FromRoute, SwaggerParameter("Patient Id")] string patientId,
            [Required, FromQuery, SwaggerParameter("Care Team Status")] CareTeamStatus status,
            [Required, FromQuery, SwaggerParameter("Care Team Category")] CareTeamCategory category)
        {
            var data = await _repository.GetCareTeamsParticipantsByPatientIdAsync(patientId, status, category);

            Validate(data);

            return Ok(data);
        }
    }
}
