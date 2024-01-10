// <copyright file="PharmacyController.cs" company="Duly Health and Care">
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
using System;
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
    public class PharmacyController : DulyControllerBase
    {
        private const string GetPreferredPharmacyByPatientIdDescription = "Returns details of Preferred Pharmacy by a Patient ";

        private readonly IPharmacyRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PharmacyController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        /// <param name="repository">A repository for working on <see cref="Appointment"/>.</param>
        public PharmacyController(
            ILogger<PharmacyController> logger,
            IWebHostEnvironment environment,
            IPharmacyRepository repository)
            : base(logger, environment)
        {
            _repository = repository;
        }

        /// <summary>
        /// Returns Preferred  <see cref="Pharmacy"/> Detail by a Patient.
        /// </summary>
        /// <param name="patientId">Patient Id.</param>
        /// <returns>Details of Preferred Pharmacy by a Patient.</returns>
        [HttpGet("{patientId}/[controller]")]
        [SwaggerOperation(
            Summary = nameof(GetPreferredPharmacyByPatientId),
            Description = GetPreferredPharmacyByPatientIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetPreferredPharmacyByPatientIdDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<Pharmacy>> GetPreferredPharmacyByPatientId(
            [Required, FromRoute, SwaggerParameter("Patient Id")] string patientId)
        {
            var data = await _repository.GetPreferredPharmacyByPatientIdAsync(patientId);

            return Ok(data);
        }

    }
}
