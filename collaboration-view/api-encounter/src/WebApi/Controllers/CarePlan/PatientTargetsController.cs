// <copyright file="PatientTargetsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan;
using Duly.Common.Annotations.Constants;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
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
    public class PatientTargetsController : DulyControllerBase
    {
        private const string DescriptionGetPatientTargets = "Get patient target";

        private const string DescriptionPostPatientTargets = "Post patient target";

        private const string DescriptionDeletePatientTarget = "Delete patient target";

        private const string DescriptionUpdatePatientTargetReviewStatus = "Update patient target review status";

        private const string DescriptionUpdatePatientTargets = "Update patient targets";

        private readonly IPatientTargetsService _patientTargetsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientTargetsController"/> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="patientTargetsService">An instance of patient targets service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public PatientTargetsController(
            IPatientTargetsService patientTargetsService,
            ILogger<PatientTargetsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _patientTargetsService = patientTargetsService;
        }

        [HttpGet("{patientPlanId}")]
        [SwaggerOperation(
        Summary = nameof(GetPatientTargetsByPatientPlanId),
        Description = DescriptionGetPatientTargets)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionGetPatientTargets, typeof(GetPatientTargets))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<GetPatientTargets>>> GetPatientTargetsByPatientPlanId(
        [Required, FromRoute, SwaggerParameter("Patient Plan ID")] long patientPlanId)
        {
            try
            {
                if (patientPlanId < 1)
                {
                    return BadRequest(new FaultResponse { ErrorMessage = "Patient Plan ID should be greater than or equal to 1." });
                }
                else
                {
                    var patientTargetResponse = await _patientTargetsService.GetPatientTargetsByPatientPlanIdAsync(patientPlanId);
                    return Ok(patientTargetResponse);
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

        [HttpPost("post-patient-targets")]
        [SwaggerOperation(
            Summary = nameof(PostPatientTargets),
            Description = DescriptionPostPatientTargets)]
        [SwaggerResponse(StatusCodes.Status201Created, DescriptionPostPatientTargets, typeof(PostPatientTargetResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Patient target for the given patient condition id and target id already exists.", Type = typeof(FaultResponse))]
        public async Task<ActionResult<PostPatientTargetResponse>> PostPatientTargets([FromBody] PatientTargets request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new FaultResponse { ErrorMessage = "Patient Plan ID should be greater than or equal to 1." });
                }
                else
                {
                    PostPatientTargetResponse postPatientTargetsResponse = new PostPatientTargetResponse();
                    var targetId = await _patientTargetsService.PostPatientTargetsAsync(request);
                    if (targetId == 0)
                    {
                        postPatientTargetsResponse.StatusCode = StatusCodes.Status409Conflict.ToString();
                        postPatientTargetsResponse.Message = "Patient target for the provided Patient Condition ID and Target ID already exists.";
                        return StatusCode(StatusCodes.Status409Conflict, postPatientTargetsResponse);
                    }
                    else if (targetId == -1)
                    {
                        postPatientTargetsResponse.StatusCode = StatusCodes.Status400BadRequest.ToString();
                        postPatientTargetsResponse.Message = "Patient Condition ID for the Condition ID provided does not exist.";
                        return StatusCode(StatusCodes.Status400BadRequest, postPatientTargetsResponse);
                    }
                    else
                    {
                        postPatientTargetsResponse.PatientTargetId = targetId;
                        postPatientTargetsResponse.StatusCode = StatusCodes.Status201Created.ToString();
                        postPatientTargetsResponse.Message = "Patient target created successfully.";
                        return StatusCode(StatusCodes.Status201Created, postPatientTargetsResponse);
                    }
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

        [HttpPost("delete-patient-target/{patientTargetId}")]
        [SwaggerOperation(
            Summary = nameof(DeletePatientTarget),
            Description = DescriptionDeletePatientTarget)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionDeletePatientTarget, typeof(DeletePatientTargetResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<DeletePatientTargetResponse>> DeletePatientTarget([FromRoute, SwaggerParameter("Patient Target Id")] long patientTargetId)
        {
            try
            {
                if (patientTargetId < 1)
                {
                    return BadRequest(new FaultResponse { ErrorMessage = "Patient Target ID should be greater than or equal to 1." });
                }
                else
                {
                    var result = new DeletePatientTargetResponse();

                    await _patientTargetsService.DeletePatientTargetAsync(patientTargetId);
                    result.IsDeleted = true;
                    result.Message = "Target is deleted";
                    result.StatusCode = StatusCodes.Status200OK.ToString();
                    return StatusCode(StatusCodes.Status200OK, result);
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

        [HttpPut("update-patient-target/{patientTargetId}")]
        [SwaggerOperation(
            Summary = nameof(UpdatePatientTargetReviewStatus),
            Description = DescriptionUpdatePatientTargetReviewStatus)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionUpdatePatientTargetReviewStatus, typeof(UpdatePatientTargetReviewStatusResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<UpdatePatientTargetReviewStatusResponse>> UpdatePatientTargetReviewStatus([FromBody] UpdatePatientTargetReviewStatus request, [FromRoute] long patientTargetId)
        {
            try
            {
                var result = new UpdatePatientTargetReviewStatusResponse();

                var responseId = await _patientTargetsService.UpdatePatientTargetReviewStatusAsync(request, patientTargetId);
                result.PatientTargetId = responseId;
                result.Message = "Review Status Updated";
                result.StatusCode = StatusCodes.Status200OK.ToString();
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

        [HttpPut("patient-target/{patientTargetId}")]
        [SwaggerOperation(
            Summary = nameof(UpdatePatientTargets),
            Description = DescriptionUpdatePatientTargets)]
        [SwaggerResponse(StatusCodes.Status202Accepted, DescriptionUpdatePatientTargets, typeof(UpdatePatientTargetsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<UpdatePatientTargets>> UpdatePatientTargets([FromBody] UpdatePatientTargets request, [FromRoute] long patientTargetId)
        {
            try
            {
                if (patientTargetId < 1)
                {
                    return BadRequest(new FaultResponse { ErrorMessage = "Patient Target ID should be greater than or equal to 1." });
                }

                var result = new UpdatePatientTargetsResponse();
                var response = await _patientTargetsService.UpdatePatientTargetsAsync(request, patientTargetId);

                if (response == -1)
                {
                    result.Message = "Patient Condition for the provided Patient Plan ID and Condition ID doesn't exists.";
                    result.StatusCode = StatusCodes.Status400BadRequest.ToString();
                    return StatusCode(StatusCodes.Status400BadRequest, result);
                }

                result.PatientTargetId = response;
                result.Message = "Patient Target Updated Successfully.";
                result.StatusCode = StatusCodes.Status202Accepted.ToString();
                return StatusCode(StatusCodes.Status202Accepted, result);
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
    }
}