// -----------------------------------------------------------------------
// <copyright file="AllergyIntoleranceController.cs" company="Duly Health and Care">
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
    public class AllergyIntoleranceController : DulyControllerBase
    {
        private const string GetAllergyIntoleranceForSpecificPatientDescription = "Returns an array of AllergyIntolerance for a specific Patient";

        private readonly IAllergyIntoleranceRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllergyIntoleranceController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger exampleProvider.</param>
        /// <param name="repository">AllergyIntolerance repository.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public AllergyIntoleranceController(
            IAllergyIntoleranceRepository repository,
            ILogger<AllergyIntoleranceController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _repository = repository;
        }

        /// <summary>
        /// Returns all available items of <see cref="Contracts.AllergyIntolerance"/>
        /// whose verification status has been confirmed for a specific Patient.
        /// </summary>
        /// <param name="patientId">Id of a specific patient.</param>
        /// <param name="status">Filter for allergy tolerance by clinical status.</param>
        /// <returns>An array of AllergyIntolerance for a specific Patient.</returns>
        [HttpGet("confirmed")]
        [SwaggerOperation(
            Summary = nameof(GetConfirmedAllergyIntoleranceForSpecificPatient),
            Description = GetAllergyIntoleranceForSpecificPatientDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetAllergyIntoleranceForSpecificPatientDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [ObfuscatedIdentityParameters(new[] { "patientId" })]
        public async Task<ActionResult<IEnumerable<AllergyIntolerance>>> GetConfirmedAllergyIntoleranceForSpecificPatient(
            [Required, FromRoute, SwaggerParameter(RoutePaths.PatientIdDescription)] string patientId,
            [Required, FromQuery, SwaggerParameter("Clinical status")] ClinicalStatus status)
        {
            var data = await _repository.GetConfirmedAllergyIntoleranceForPatientAsync(patientId, status);

            Validate(data);

            return Ok(data);
        }
    }
}