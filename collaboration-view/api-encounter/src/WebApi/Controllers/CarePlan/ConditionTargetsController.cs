// <copyright file="ConditionTargetsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan;
using Duly.Common.Annotations.Constants;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
using Duly.Common.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Controllers.CarePlan
{
    [Route("care-plan/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class ConditionTargetsController : DulyControllerBase
    {
        private const string DescriptionConditionTargets = "Get condition targets by condition id.";

        private readonly IConditionTargetsService _conditionTargetsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionTargetsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="conditionTargetsService">An instance of condition targets service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public ConditionTargetsController(
            IConditionTargetsService conditionTargetsService,
            ILogger<ConditionTargetsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _conditionTargetsService = conditionTargetsService;
        }

        [HttpGet("/care-plan/health-targets")]
        [SwaggerOperation(
            Summary = nameof(GetTargetsForConditions),
            Description = DescriptionConditionTargets)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionConditionTargets, typeof(IEnumerable<GetConditionTargetsResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<GetConditionTargetsResponse>>> GetTargetsForConditions([Required, FromQuery] string conditionIds)
        {
            try
            {
                var result = await _conditionTargetsService.GetConditionTargetsByConditionId(conditionIds);
                return Ok(result);
            }
            catch (SqlException ex)
            {
                Logger.LogWarning(ex, ex.Message);
                return BadRequest(new FaultResponse { ErrorMessage = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                Logger.LogWarning(ex, ex.Message);
                return NotFound(new FaultResponse { ErrorMessage = ex.Message });
            }
            catch (Exception e)
            {
                Logger.LogWarning(e, e.Message);
                return BadRequest(new FaultResponse { ErrorMessage = e.Message });
            }
        }
    }
}