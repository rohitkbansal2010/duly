// <copyright file="CarePlanDetailsController.cs" company="Duly Health and Care">
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
using System;
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
    public class CarePlanDetailsController : DulyControllerBase
    {
        private const string CarePlanDetailsDescription = "Get Care Plan Details.";

        private readonly ICarePlanDetailsRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarePlanDetailsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        /// <param name="repository">A repository for working with <see cref="CarePlanDetails"/>.</param>
        public CarePlanDetailsController(
            ILogger<CarePlanDetailsController> logger,
            IWebHostEnvironment environment,
            ICarePlanDetailsRepository repository)
            : base(logger, environment)
        {
            _repository = repository;
        }

        [HttpGet("[controller]/{patientId}")]
        [SwaggerOperation(
           Summary = nameof(GetCarePlanDetails),
           Description = CarePlanDetailsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, CarePlanDetailsDescription, typeof(IEnumerable<CarePlanDetails>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<CarePlanDetails>>> GetCarePlanDetails(
            [Required, FromRoute, SwaggerParameter("Patient Id")] string patientId)
        {
            var result = await _repository.GetCarePlanDetailsAsync(patientId);
            return Ok(result);
        }
    }
}