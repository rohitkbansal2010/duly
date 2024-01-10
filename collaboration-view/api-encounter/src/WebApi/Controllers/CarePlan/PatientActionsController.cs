// <copyright file="PatientActionsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Controllers.CarePlan
{
    [Route("care-plan/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class PatientActionsController : DulyControllerBase
    {
        private const string DescriptionGetPatientActionsByPatientTargetId = "Get Patient actions data by patient target id";

        private const string DescriptionSavePatientActions = "Save Patientactions data";

        private const string DescriptionUpdateActionProgress = "Update Action Progress";

        private const string DescriptionGetPatientActionsStats = "Get Count of Distinct Actions, Total Actions and other stats on the basis of patient plan id.";

        private readonly IPatientActionsService _patientActionsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientActionsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="patientActionsService">An instance of Patient Actions service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public PatientActionsController(
               IPatientActionsService patientActionsService,
               ILogger<PatientActionsController> logger,
               IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _patientActionsService = patientActionsService;
        }

        [HttpGet("{patientTargetId}")]
        [SwaggerOperation(
            Summary = nameof(GetPatientActionsByPatientTargetId),
            Description = DescriptionGetPatientActionsByPatientTargetId)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionGetPatientActionsByPatientTargetId, typeof(GetPatientActions))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<GetPatientActions>>> GetPatientActionsByPatientTargetId(
            [Required, FromRoute, SwaggerParameter("Patient Target ID")] long patientTargetId)
        {
            try
            {
                if (patientTargetId > 0)
                {
                    var getPatientActionsResponse = await _patientActionsService.GetPatientActionsByPatientTargetIdAsync(patientTargetId);
                    return Ok(getPatientActionsResponse);
                }
                else
                {
                    return BadRequest(new FaultResponse { ErrorMessage = "Patient target id must be greater than zero." });
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

        [HttpPost("post-patient-actions")]
        [SwaggerOperation(
            Summary = nameof(PostPatientActions),
            Description = DescriptionSavePatientActions)]
        [SwaggerResponse(StatusCodes.Status201Created, DescriptionSavePatientActions, typeof(PostPatientActionsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [SwaggerOperationFilter(typeof(RequestBodyDescriptionFilter))]
        public async Task<ActionResult<PostPatientActionsResponse>> PostPatientActions(IEnumerable<PatientActions> request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new FaultResponse { ErrorMessage = "Patient action is required." });
                }
                else
                {
                    var result = new PostPatientActionsResponse();
                    var patientTargetId = await _patientActionsService.PostPatientActionsAsync(request);
                    result.PatientTargetId = patientTargetId;
                    result.Message = "Patient actions created successfully.";
                    result.StatusCode = StatusCodes.Status201Created;

                    return StatusCode(StatusCodes.Status201Created, result);
                }
            }
            catch (SqlException ex)
            {
                Logger.LogWarning(ex, ex.Message);
                return BadRequest(new FaultResponse { ErrorMessage = ex.Message });
            }
            catch (Exception e)
            {
                Logger.LogWarning(e, e.Message);
                return BadRequest(new FaultResponse { ErrorMessage = e.Message });
            }
        }

        [HttpPut("update-action-progress/{patientActionId}")]
        [SwaggerOperation(
            Summary = nameof(UpdateActionProgress),
            Description = DescriptionUpdateActionProgress)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionUpdateActionProgress, typeof(UpdateActionProgressResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [SwaggerOperationFilter(typeof(RequestBodyDescriptionFilter))]
        public async Task<ActionResult<UpdateActionProgressResponse>> UpdateActionProgress([FromBody] UpdateActionProgress request, [FromRoute] long patientActionId)
        {
            try
            {
                var result = new UpdateActionProgressResponse();
                var responseId = await _patientActionsService.UpdateActionProgressAsync(request, patientActionId);
                result.PatientActionId = responseId;
                result.Message = "Patient actions updated successfully.";
                result.StatusCode = StatusCodes.Status200OK;
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (SqlException ex)
            {
                Logger.LogWarning(ex, ex.Message);
                return BadRequest(new FaultResponse { ErrorMessage = ex.Message });
            }
            catch (Exception e)
            {
                Logger.LogWarning(e, e.Message);
                return BadRequest(new FaultResponse { ErrorMessage = e.Message });
            }
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(GetPatientActionStats),
            Description = DescriptionGetPatientActionsStats)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionGetPatientActionsStats, typeof(GetPatientActionStats))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<GetPatientActionStats>> GetPatientActionStats(
            [Required, FromQuery, SwaggerParameter("Patient Plan ID")] long patientPlanId)
        {
            try
            {
                if (patientPlanId > 0)
                {
                    var response = await _patientActionsService.GetPatientActionStatsAsync(patientPlanId);
                    return Ok(response);
                }
                else
                {
                    return BadRequest(new FaultResponse { ErrorMessage = "Patient plan id must be greater than zero." });
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