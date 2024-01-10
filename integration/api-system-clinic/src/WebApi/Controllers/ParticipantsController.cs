// <copyright file="ParticipantsController.cs" company="Duly Health and Care">
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
    [Route(RoutePaths.DefaultEncounterRoute)]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    public class ParticipantsController : DulyControllerBase
    {
        private const string GetParticipantsByEncounterIdDescription = "Returns a list of participants involved in the encounter";

        private readonly IParticipantRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticipantsController" /> class.
        /// </summary>
        /// <param name="repository">A repository for working on <see cref="Participant"/>.</param>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public ParticipantsController(
            IParticipantRepository repository,
            ILogger<ParticipantsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _repository = repository;
        }

        /// <summary>
        /// Returns list of <see cref="Participant"/> involved in the encounter.
        /// </summary>
        /// <param name="encounterId">Id of a specific encounter.</param>
        /// <returns>Returns list of participants involved in the encounter.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(GetParticipantsByEncounterId),
            Description = GetParticipantsByEncounterIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, "List participants involved in the encounter.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [ObfuscatedIdentityParameters(new[] { "encounterId" })]
        public async Task<ActionResult<IEnumerable<Participant>>> GetParticipantsByEncounterId(
            [Required, FromRoute, SwaggerParameter("Encounter Id")] string encounterId)
        {
            var data = await _repository.GetParticipantsByEncounterIdAsync(encounterId);

            Validate(data);

            return Ok(data);
        }
    }
}
