// -----------------------------------------------------------------------
// <copyright file="MedicationsController.cs" company="Duly Health and Care">
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
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Controllers
{
    [Route(RoutePaths.DefaultPatientRoute)]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class MedicationsController : DulyControllerBase
    {
        private const string GetMedicationsByPatientIdDescription = "Returns an information for each active medication for a specific patient.";

        private readonly IMedicationService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicationsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="service">An instance of Medication service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public MedicationsController(
            IMedicationService service,
            ILogger<MedicationsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _service = service;
        }

        /// <summary>
        /// Returns <see cref="Medications"/> that represents information about medications
        /// that the specific patient is/was taking, divided by the type of schedule.
        /// </summary>
        /// <param name="patientId">Id of a specific patient.</param>
        /// <returns>An instance of <see cref="Medications"/> for a specific patient.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(GetMedicationsByPatientId),
            Description = GetMedicationsByPatientIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetMedicationsByPatientIdDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<Medications>> GetMedicationsByPatientId(
            [Required, FromRoute, SwaggerParameter(RoutePaths.PatientIdDescription)] string patientId)
        {
            var medications = await _service.GetMedicationsByPatientIdAsync(patientId);

            Validate(medications);

            return Ok(medications);
        }

        /// <summary>
        /// Returns <see cref="Medications"/> that represents information about medications
        /// that the specific patient is/was taking, divided by the type of schedule.
        /// </summary>
        /// <param name="patientId">Id of a specific patient.</param>
        /// <param name="appointmentId">Id of a specific appointmentId.</param>
        /// <returns>An instance of <see cref="Medications"/> for a specific patient.</returns>
        [HttpGet("/patientId/{patientId}/appointmentId/{appointmentId}")]
        [SwaggerOperation(
        Summary = nameof(GetMedicationsRequestByappointmentId),
        Description = GetMedicationsByPatientIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetMedicationsByPatientIdDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<Medications>> GetMedicationsRequestByappointmentId(
        [Required, FromRoute, SwaggerParameter(RoutePaths.PatientIdDescription)] string patientId,
        [Required, FromRoute] string appointmentId)
        {
            var medications = await _service.GetMedicationsRequestByPatientIdAsync(patientId, appointmentId);

            Validate(medications);

            return Ok(medications);
        }
    }
}
