// <copyright file="PartiesController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Annotations.Constants;
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
    [Route("patients/{patientId}/appointments/{appointmentId}/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class PartiesController : DulyControllerBase
    {
        private const string GetPartiesByAppointmentAndPatientIdsDescription = "Returns an array of " +
                                                                               "Members (except patient) who are associated with current appointment and patient;\n\n" +
                                                                               "Patient's care team.\n\n" +
                                                                               "The list does not contain duplications\n\n" +
                                                                               "The list is ordered\n\n";

        private readonly IPartyService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartiesController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="service">An instance of Party service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public PartiesController(
            IPartyService service,
            ILogger<PartiesController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _service = service;
        }

        /// <summary>
        /// Returns all available <see cref="Party"/> for the patient and the appointment.
        /// </summary>
        /// <param name="patientId">Id of a specific patient.</param>
        /// <param name="appointmentId">Id of a specific appointment.</param>
        /// <returns>Returns Parties of concrete patient and appointment.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(GetPartiesByPatientAndAppointmentId),
            Description = GetPartiesByAppointmentAndPatientIdsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns Parties of the patient and the appointment.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<Party>>> GetPartiesByPatientAndAppointmentId(
            [Required, FromRoute, SwaggerParameter("Patient Id")] string patientId,
            [Required, FromRoute, SwaggerParameter("Appointment Id")] string appointmentId)
        {
            var data = await _service.GetPartiesByPatientAndAppointmentIdAsync(patientId, appointmentId);

            Validate(data);

            return Ok(data);
        }
    }
}
