// <copyright file="ConditionTargetsController.cs" company="Duly Health and Care">
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
    public class ConditionTargetsController : DulyControllerBase
    {
        private const string ConditionTargetsDescription = "Get health condition targets by condition ids.";

        private readonly IConditionTargetsRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionTargetsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        /// <param name="repository">A repository for working with <see cref="ConditionTargets"/>.</param>
        public ConditionTargetsController(
            ILogger<ConditionTargetsController> logger,
            IWebHostEnvironment environment,
            IConditionTargetsRepository repository)
            : base(logger, environment)
        {
            _repository = repository;
        }

        [HttpGet("[controller]")]
        [SwaggerOperation(
           Summary = nameof(GetTargetsForConditions),
           Description = ConditionTargetsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, ConditionTargetsDescription, typeof(IEnumerable<ConditionTargets>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<ConditionTargets>>> GetTargetsForConditions([Required, FromQuery] string conditionIds)
        {
            var result = await _repository.GetConditionTargetsByConditionIdAsync(conditionIds);
            return Ok(result);
        }
    }
}
