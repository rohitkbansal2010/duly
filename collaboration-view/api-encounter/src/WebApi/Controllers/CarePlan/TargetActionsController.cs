// <copyright file="TargetActionsController.cs" company="Duly Health and Care">
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
    public class TargetActionsController : DulyControllerBase
    {
        private const string DescriptionTargetActions = "Get actions by target id.";

        private readonly ITargetActionsService _targetActionsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetActionsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="targetActionsService">An instance of target actions service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public TargetActionsController(
            ITargetActionsService targetActionsService,
            ILogger<TargetActionsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _targetActionsService = targetActionsService;
        }

        [HttpGet("/care-plan/actions")]
        [SwaggerOperation(
            Summary = nameof(GetActionsForTarget),
            Description = DescriptionTargetActions)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionTargetActions, typeof(IEnumerable<TargetActions>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<TargetActions>>> GetActionsForTarget([Required, FromQuery, SwaggerParameter("Condition Id")] long conditionId, [Required, FromQuery, SwaggerParameter("Target Id")] long targetId)
        {
            try
            {
                if (conditionId > 0 && targetId > 0)
                {
                    var result = await _targetActionsService.GetTargetActionsByConditionIdAndTargetIdAsync(conditionId, targetId);
                    return Ok(result);
                }
                else
                {
                    return BadRequest(new FaultResponse { ErrorMessage = "Either Condition id or Target id is invalid." });
                }
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