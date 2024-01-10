// <copyright file="PatientsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Common.Annotations.Constants;
using Duly.Common.ApiExtensions.Constants;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
using Duly.Common.Infrastructure.Exceptions;
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
    [Route("[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    public class PatientsController : DulyControllerBase
    {
        private const string DescriptionGetPatient = "Returns a patient with details";
        private const string DescriptionGetPatients = "Returns patients with details";
        private const string DescriptionGetPatientsPhotoById = "Returns patient photos by id";
        private readonly IPatientRepository _patientRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientsController" /> class.
        /// </summary>
        /// <param name="patientRepository">A repository for working on <see cref="Patient"/>.</param>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public PatientsController(
            IPatientRepository patientRepository,
            ILogger<PatientsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _patientRepository = patientRepository;
        }

        /// <summary>
        /// Returns a <see cref="Patient"/> with details.
        /// </summary>
        /// <param name="patientId">Id of a specific patient.</param>
        /// <returns>Returns a patient with details.</returns>
        [HttpGet(RoutePaths.PatientIdName)]
        [SwaggerOperation(Summary = nameof(GetPatientById), Description = DescriptionGetPatient)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionGetPatient)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [ObfuscatedIdentityParameters(new[] { "patientId" })]
        public async Task<ActionResult<Patient>> GetPatientById(
            [Required, FromRoute, SwaggerParameter(RoutePaths.PatientIdDescription)] string patientId)
        {
            var data = await _patientRepository.GetPatientByIdAsync(patientId);

            if (data == null)
            {
                throw new EntityNotFoundException(nameof(Patient), patientId);
            }

            Validate(data);

            return Ok(data);
        }

        /// <summary>
        /// Returns all available items of <see cref="Patient"/> with details for a set of identifiers.
        /// </summary>
        /// <param name="identifiers">Identifiers of patients.</param>
        /// <returns>Returns patients with details.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = nameof(GetPatientsByIdentifiers), Description = DescriptionGetPatients)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionGetPatients)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatientsByIdentifiers(
            [Required, FromQuery, SwaggerParameter("identifiers")] string[] identifiers)
        {
            var data = await _patientRepository.GetPatientsByIdentifiersAsync(identifiers);

            Validate(data);

            return Ok(data);
        }

        /// <summary>
        /// Returns all available patient photo by id.
        /// </summary>
        /// <param name="param">Identifiers of patients.</param>
        /// <returns>Returns patient photos.</returns>
        [HttpPost("/patients/getpatientphotobyid")]
        [SwaggerOperation(Summary = nameof(GetPatientsPhotoById), Description = DescriptionGetPatientsPhotoById)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionGetPatientsPhotoById)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<List<PatientPhoto>>> GetPatientsPhotoById(
            [Required, FromBody, SwaggerParameter("Identifiers")] PatientPhotoByParam param)
        {
            var data = await _patientRepository.GetPatientsPhotoByIdAsync(param);
            return Ok(data);
        }
    }
}