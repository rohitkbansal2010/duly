// <copyright file="PatientActionsController.cs" company="Duly Health and Care">
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
    public class PatientActionsController : DulyControllerBase
    {
        private const string GetPatientActionsByTargetIdDescription = "Get Patient Actions of the Targets by Target ID.";
        private const string CreatePatientActionsDescription = "Save Patient Actions of the Targets.";

        private readonly IPatientActionsRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientActionsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        /// <param name="repository">A repository for working with <see cref=""/>.</param>
        public PatientActionsController(
                   IPatientActionsRepository repository,
                   ILogger<CustomTargetsController> logger,
                   IWebHostEnvironment environment)
                   : base(logger, environment)
        {
            _repository = repository;
        }

        [HttpGet("[controller]/{patientTargetId}")]
        [SwaggerOperation(
        Summary = nameof(GetPatientActions),
        Description = GetPatientActionsByTargetIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetPatientActionsByTargetIdDescription, typeof(IEnumerable<GetPatientActions>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<GetPatientActions>>> GetPatientActions(
        [Required, FromRoute, SwaggerParameter("Patient Target Id")] long patientTargetId)
        {
            var result = await _repository.GetPatientActionsAsync(patientTargetId);
            return Ok(result);
        }

        [HttpPost("[controller]")]
        [SwaggerOperation(
             Summary = nameof(PostPatientActions),
             Description = CreatePatientActionsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, CreatePatientActionsDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<long>> PostPatientActions(IEnumerable<PatientActions> request)
        {
            var data = await _repository.PostPatientActionsAsync(request);

            return Ok(data);
        }
    }
}
