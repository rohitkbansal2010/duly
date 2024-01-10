// <copyright file="PatientConditionsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Annotations.Constants;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
using Duly.Ngdp.Api.Repositories.Interfaces.CarePlan;
using Duly.Ngdp.Contracts.CarePlan;
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

namespace Duly.Ngdp.Api.Controllers.CarePlan
{
    [ApiController]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class PatientConditionsController : DulyControllerBase
    {
        private const string GetPatientConditionsDescription = "Get patient conditions data of the patient.";
        private const string CreatePatientConditionsDescription = "Save patient conditions data of the patient.";

        private readonly IPatientConditionsRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientConditionsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        /// <param name="repository">A repository for working with <see cref=""/>.</param>
        public PatientConditionsController(
                   IPatientConditionsRepository repository,
                   ILogger<PatientConditionsController> logger,
                   IWebHostEnvironment environment)
                   : base(logger, environment)
        {
            _repository = repository;
        }

        [HttpGet("[controller]/{patientPlanId}")]
        [SwaggerOperation(
           Summary = nameof(GetPatientConditions),
           Description = GetPatientConditionsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetPatientConditionsDescription, typeof(IEnumerable<GetPatientConditions>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<GetPatientConditions>>> GetPatientConditions(
            [Required, FromRoute, SwaggerParameter("Patient Plan Id")] long patientPlanId)
        {
            var result = await _repository.GetPatientConditionsAsync(patientPlanId);
            return Ok(result);
        }

        [HttpPost("[controller]")]
        [SwaggerOperation(
             Summary = nameof(PostPatientConditions),
             Description = CreatePatientConditionsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, CreatePatientConditionsDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<long>> PostPatientConditions([FromBody] PatientConditions request)
        {
            var data = await _repository.PostPatientConditionsAsync(request);

            return Ok(data);
        }
    }
}
