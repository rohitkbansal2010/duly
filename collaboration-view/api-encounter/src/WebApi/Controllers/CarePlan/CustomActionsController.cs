// <copyright file="CustomActionsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan;
using Duly.Common.Annotations.Constants;
using Duly.Common.Annotations.Filters;
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
    public class CustomActionsController : DulyControllerBase
    {
        private const string DescriptionSaveCustomActions = "Save CustomActions data";

        private readonly ICustomActionsService _customActionsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomActionsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="customActionsService">An instance of Custom Actions service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public CustomActionsController(
               ICustomActionsService customActionsService,
               ILogger<CustomActionsController> logger,
               IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _customActionsService = customActionsService;
        }

        [HttpPost("post-custom-actions")]
        [SwaggerOperation(
            Summary = nameof(PostCustomActions),
            Description = DescriptionSaveCustomActions)]
        [SwaggerResponse(StatusCodes.Status201Created, DescriptionSaveCustomActions, typeof(PostCustomActionsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [SwaggerOperationFilter(typeof(RequestBodyDescriptionFilter))]
        public async Task<ActionResult<PostCustomActionsResponse>> PostCustomActions([FromBody] CustomActions request)
        {
            try
            {
                if (request.PatientTargetId <= 0)
                {
                    return BadRequest(new FaultResponse { ErrorMessage = "Patient Target ID is required." });
                }
                else
                {
                    var result = new PostCustomActionsResponse();

                    var customActionId = await _customActionsService.PostCustomActionsAsync(request);
                    result.CustomActionId = customActionId;
                    result.Message = "Custom action created successfully";
                    result.StatusCode = StatusCodes.Status201Created;

                    return StatusCode(StatusCodes.Status201Created, result);
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