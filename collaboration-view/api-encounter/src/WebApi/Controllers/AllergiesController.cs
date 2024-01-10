// -----------------------------------------------------------------------
// <copyright file="AllergiesController.cs" company="Duly Health and Care">
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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Controllers
{
    /// <summary>
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-367.
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-1345.
    /// </summary>
    [Route(RoutePaths.DefaultPatientRoute)]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class AllergiesController : DulyControllerBase
    {
        private const string GetAllergiesForSpecificPatientDescription = "Returns an array of Allergies for a specific Patient";

        private readonly IAllergyService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllergiesController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="service">An instance of Allergy service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public AllergiesController(
            IAllergyService service,
            ILogger<AllergiesController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _service = service;
        }

        /// <summary>
        /// Returns all available <see cref="Allergy"/> for a specific Patient.
        /// </summary>
        /// <param name="patientId">Id of Patient.</param>
        /// <returns>An array of Allergies for a specific Patient.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(GetAllergiesForSpecificPatient),
            Description = GetAllergiesForSpecificPatientDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetAllergiesForSpecificPatientDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<Allergy>>> GetAllergiesForSpecificPatient(
            [Required, FromRoute, SwaggerParameter(RoutePaths.PatientIdDescription)] string patientId)
        {
            var allergies = await _service.GetAllergiesForPatientAsync(patientId);

            Validate(allergies);

            return Ok(allergies);
        }
    }
}
