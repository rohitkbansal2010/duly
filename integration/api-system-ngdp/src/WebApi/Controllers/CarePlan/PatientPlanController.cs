// <copyright file="PatientPlanController.cs" company="Duly Health and Care">
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
    public class PatientPlanController : DulyControllerBase
    {
        private const string CreatePatientPlanDescription = "Save patient plan data of the patient.";
        private const string UpdatePatientPlanStatusByIdDescription = "Update patient plan status by id.";
        private const string GetPatientPlanDetailsDescription = "Get Patient Plan Details by Patient ID.";

        private readonly IPatientPlanRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientPlanController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        /// <param name="repository">A repository for working with <see cref=""/>.</param>
        public PatientPlanController(
                   IPatientPlanRepository repository,
                   ILogger<PatientPlanController> logger,
                   IWebHostEnvironment environment)
                   : base(logger, environment)
        {
            _repository = repository;
        }

        [HttpGet("[controller]/{patientId}")]
        [SwaggerOperation(
        Summary = nameof(GetPatientPlan),
        Description = GetPatientPlanDetailsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetPatientPlanDetailsDescription, typeof(IEnumerable<GetPatientPlan>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<PatientPlanDetails>>> GetPatientPlan(
        [Required, FromRoute, SwaggerParameter("Patient Id")] string patientId)
        {
            var result = await _repository.GetPatientPlanAsync(patientId);
            return Ok(result);
        }

        [HttpPost("[controller]")]
        [SwaggerOperation(
             Summary = nameof(PostPatientPlan),
             Description = CreatePatientPlanDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, CreatePatientPlanDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<int>> PostPatientPlan([FromBody] PatientPlan request)
        {
            var data = await _repository.PostPatientPlanAsync(request);

            return Ok(data);
        }

        [HttpPost("UpdatePatientPlanStatusById/{id}")]
        [SwaggerOperation(
           Summary = nameof(UpdatePatientPlanStatusById),
           Description = UpdatePatientPlanStatusByIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, UpdatePatientPlanStatusByIdDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<bool>> UpdatePatientPlanStatusById([FromRoute, SwaggerParameter("Patient Plan Id")] long id)
        {
            var data = await _repository.UpdatePatientPlanStatusByIdAsync(id);

            return Ok(data);
        }
    }
}
