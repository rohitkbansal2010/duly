// <copyright file="ImmunizationsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Annotations.Constants;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
using Duly.Ngdp.Api.Repositories.Interfaces;
using Duly.Ngdp.Contracts;
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

namespace Duly.Ngdp.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class ImmunizationsController : DulyControllerBase
    {
        private const string GetImmunizationsForSpecificPatientDescription = "Returns an array of immunizations for a specific patient";

        private readonly IImmunizationRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmunizationsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        /// <param name="repository">A repository for working on <see cref="Appointment"/>.</param>
        public ImmunizationsController(
            ILogger<ImmunizationsController> logger,
            IWebHostEnvironment environment,
            IImmunizationRepository repository)
            : base(logger, environment)
        {
            _repository = repository;
        }

        /// <summary>
        /// Returns all available <see cref="Immunization"/> for a specific patient.
        /// </summary>
        /// <param name="patientId">Patient Id.</param>
        /// <param name="includedDueStatuses">Due statuses that should be included.</param>
        /// <returns>An array of immunizations for specific patient.</returns>
        [HttpGet("patients/{patientId}/[controller]")]
        [SwaggerOperation(
            Summary = nameof(GetImmunizationsForSpecificPatient),
            Description = GetImmunizationsForSpecificPatientDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetImmunizationsForSpecificPatientDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<Immunization>>> GetImmunizationsForSpecificPatient(
            [Required, FromRoute, SwaggerParameter("Patient Id")] string patientId,
            [Required, FromQuery, SwaggerParameter("Due Statuses which should be included")] DueStatus[] includedDueStatuses)
        {
            var data = await _repository.GetImmunizationsForSpecificPatientAsync(patientId, includedDueStatuses);

            Validate(data);

            return Ok(data);
        }
    }
}
