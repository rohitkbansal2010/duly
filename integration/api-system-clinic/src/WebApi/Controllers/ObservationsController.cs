// <copyright file="ObservationsController.cs" company="Duly Health and Care">
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
    public class ObservationsController : DulyControllerBase
    {
        private const string GetObservationsByPatientIdDescription = "Returns observations with details";
        private const string GetLastObservationsByPatientIdDescription = "Returns last observations with details";
        private const string VitalSignsRoute = "vitalSigns";

        private readonly IObservationRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservationsController" /> class.
        /// </summary>
        /// <param name="repository">A repository for working on <see cref="Observation"/>.</param>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public ObservationsController(
            IObservationRepository repository,
            ILogger<ObservationsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _repository = repository;
        }

        /// <summary>
        /// Return a <see cref="Observation"/> with details list.
        /// </summary>
        /// <param name="patientId">Id of a specific patient.</param>
        /// <param name="observationTypes">Filter for observation by type.</param>
        /// <param name="lowerBoundDate">Lower bound of the range of the filter. [2012-12-12;...].</param>
        /// <param name="upperBoundBoundDate">Upper bound of the range of the filter.[;2022-12-12].</param>
        /// <returns>Return observations with details.</returns>
        [HttpGet(VitalSignsRoute)]
        [SwaggerOperation(
            Summary = nameof(GetObservationsByPatientId),
            Description = GetObservationsByPatientIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, "Observations with details.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [ObfuscatedIdentityParameters(new[] { "patientId" })]
        public async Task<ActionResult<IEnumerable<Observation>>> GetObservationsByPatientId(
            [Required, FromRoute, SwaggerParameter("Patient Id")] string patientId,
            [Required, FromQuery, SwaggerParameter("Observation Types")] ObservationType[] observationTypes,
            [Required, FromQuery, SwaggerParameter("Lower Bound Date of Range")] DateTime lowerBoundDate,
            [Required, FromQuery, SwaggerParameter("Upper Bound Date of Range")] DateTime upperBoundBoundDate)
        {
            var data = await _repository.FindObservationsForPatientAsync(patientId, observationTypes, lowerBoundDate, upperBoundBoundDate);

            Validate(data);

            return Ok(data);
        }

        /// <summary>
        /// Return a last <see cref="Observation"/> with details list.
        /// </summary>
        /// <param name="patientId">Id of a specific patient.</param>
        /// <param name="observationTypes">Filter for observation by type.</param>
        /// <returns>Return last observations with details.</returns>
        [HttpGet(VitalSignsRoute + "/last")]
        [SwaggerOperation(
            Summary = nameof(GetLastObservationsByPatientId),
            Description = GetLastObservationsByPatientIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, "Last observations with details.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [ObfuscatedIdentityParameters(new[] { "patientId" })]
        public async Task<ActionResult<IEnumerable<Observation>>> GetLastObservationsByPatientId(
            [Required, FromRoute, SwaggerParameter("Patient Id")] string patientId,
            [Required, FromQuery, SwaggerParameter("Observation Types")] ObservationType[] observationTypes)
        {
            var data = await _repository.FindLastObservationsForPatientAsync(patientId, observationTypes);

            Validate(data);

            return Ok(data);
        }
    }
}
