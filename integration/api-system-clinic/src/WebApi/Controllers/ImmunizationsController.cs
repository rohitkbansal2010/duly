// <copyright file="ImmunizationsController.cs" company="Duly Health and Care">
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
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Controllers
{
    [Route(RoutePaths.DefaultPatientRoute)]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    public class ImmunizationsController : DulyControllerBase
    {
        private readonly IImmunizationRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmunizationsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        /// <param name="repository">A repository for working on <see cref="Immunization"/>.</param>
        public ImmunizationsController(
            ILogger<ImmunizationsController> logger,
            IWebHostEnvironment environment,
            IImmunizationRepository repository)
            : base(logger, environment)
        {
            _repository = repository;
        }

        /// <summary>
        /// Return an <see cref="Immunization"/> with details list.
        /// </summary>
        /// <param name="patientId">Id of a specific patient.</param>
        /// <param name="immunizationStatuses">Filter for immunization statuses.</param>
        /// <returns>Return immunizations with details.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(FindImmunizationsForPatient),
            Description = "Finds past immunizations for the patient with provided Id, optionally filters by immunization status")]
        [SwaggerResponse(StatusCodes.Status200OK, "Immunizations with details.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [ObfuscatedIdentityParameters(new[] { "patientId" })]
        public async Task<ActionResult<IEnumerable<Immunization>>> FindImmunizationsForPatient(
            [Required, FromRoute, SwaggerParameter("Patient Id")] string patientId,
            [FromQuery, SwaggerParameter("Immunization statuses")] ImmunizationStatus[] immunizationStatuses = null)
        {
            var data = await _repository.FindImmunizationsForPatientAsync(
                patientId,
                immunizationStatuses?.Any() ?? false ? immunizationStatuses : null);

            Validate(data);

            return Ok(data);
        }
    }
}
