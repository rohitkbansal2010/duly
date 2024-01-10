// <copyright file="PatientTargetsController.cs" company="Duly Health and Care">
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
    public class PatientTargetsController : DulyControllerBase
    {
        private const string GetPatientTargetDescription = "Get patient targets by Patient Plan Identifier";

        private const string CreatePatientTargetDescription = "Create patient targets";

        private const string DeletePatientTargetDescription = "Delete patient target";


        private readonly IPatientTargetsRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientTargetsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        /// <param name="repository">A repository for working with <see cref=""/>.</param>
        public PatientTargetsController(
            ILogger<PatientTargetsController> logger,
            IWebHostEnvironment environment,
            IPatientTargetsRepository repository)
            : base(logger, environment)
        {
            _repository = repository;
        }

        [HttpGet("[controller]/{patientPlanId}")]
        [SwaggerOperation(
        Summary = nameof(GetPatientTargets),
        Description = GetPatientTargetDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetPatientTargetDescription, typeof(IEnumerable<GetPatientTargets>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<GetPatientTargets>>> GetPatientTargets(
        [Required, FromRoute, SwaggerParameter("Patient Plan Id")] long patientPlanId)
        {
            var result = await _repository.GetPatientTargetsAsync(patientPlanId);
            return Ok(result);
        }


        [HttpPost("[controller]")]
        [SwaggerOperation(
            Summary = nameof(PostPatientTargets),
            Description = CreatePatientTargetDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, CreatePatientTargetDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<int>> PostPatientTargets(PatientTarget request)
        {
            var data = await _repository.PostPatientTargetsAsync(request);

            return Ok(data);
        }

        [HttpPost("DeletePatientTarget/{id}")]
        [SwaggerOperation(
           Summary = nameof(DeletePatientTarget),
           Description = DeletePatientTargetDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, DeletePatientTargetDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<int>> DeletePatientTarget([FromRoute, SwaggerParameter("Patient Target Id")] long id)
        {
            var data = await _repository.DeletePatientTargetAsync(id);

            return Ok(data);
        }
    }
}