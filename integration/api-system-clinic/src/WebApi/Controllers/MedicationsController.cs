// <copyright file="MedicationsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Common.ApiExtensions.Constants;
using Duly.Common.ApiExtensions.Controllers;
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
    public class MedicationsController : DulyControllerBase
    {
        private const string GetMedicationsByPatientIdDescription = "Returns medications with details";

        private readonly IMedicationRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicationsController" /> class.
        /// </summary>
        /// <param name="repository">A repository for working on <see cref="Medication"/>.</param>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public MedicationsController(
            IMedicationRepository repository,
            ILogger<MedicationsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _repository = repository;
        }

        /// <summary>
        /// Return a <see cref="Medication"/> with details list.
        /// </summary>
        /// <param name="patientId">Id of a specific patient.</param>
        /// <param name="medicationStatus">Filter for medication.</param>
        /// <param name="medicationCategories">Filter for medication categories.</param>
        /// <returns>Return medications with details.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(GetMedicationsByPatientId),
            Description = GetMedicationsByPatientIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, "Medications with details.")]
        [ObfuscatedIdentityParameters(new[] { "patientId" })]
        public async Task<ActionResult<IEnumerable<Medication>>> GetMedicationsByPatientId(
            [Required, FromRoute, SwaggerParameter("Patient Id")] string patientId,
            [FromQuery, SwaggerParameter("Medication status")] MedicationStatus? medicationStatus = null,
            [FromQuery, SwaggerParameter("Medication categories")] MedicationCategory[] medicationCategories = null)
        {
            var data = await _repository.FindMedicationsForPatientAsync(
                patientId,
                medicationStatus,
                medicationCategories?.Any() ?? false ? medicationCategories : null);

            Validate(data);

            return Ok(data);
        }

        /// <summary>
        /// Return a <see cref="Medication"/> with details list.
        /// </summary>
        /// <param name="patientId">Id of a specific patient.</param>
        /// <param name="appointmentId">Id of an appointment.</param>
        /// <param name="medicationStatus">Filter for medication.</param>
        /// <param name="medicationCategories">Filter for medication categories.</param>
        /// <returns>Return medications with details.</returns>
        [HttpGet("/patientId/{patientId}/appointmentId/{appointmentId}")]
        [SwaggerOperation(
            Summary = nameof(GetMedicationsRequestByAppointmentId),
            Description = GetMedicationsByPatientIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, "Medications with details.")]
        [ObfuscatedIdentityParameters(new[] { "patientId" })]
        public async Task<ActionResult<IEnumerable<Medication>>> GetMedicationsRequestByAppointmentId(
            [Required, FromRoute, SwaggerParameter("Patient Id")] string patientId,
            [Required, FromRoute, SwaggerParameter("Appointment Id")] string appointmentId,
            [FromQuery, SwaggerParameter("Medication status")] MedicationStatus? medicationStatus = null,
            [FromQuery, SwaggerParameter("Medication categories")] MedicationCategory[] medicationCategories = null)
        {
            var data = await _repository.FindMedicationsRequestForPatientAsync(
                patientId,
                appointmentId,
                medicationStatus,
                medicationCategories?.Any() ?? false ? medicationCategories : null
                );

            Validate(data);

            return Ok(data);
        }
    }
}
