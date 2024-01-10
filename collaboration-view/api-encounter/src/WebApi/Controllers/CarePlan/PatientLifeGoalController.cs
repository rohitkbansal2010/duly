// <copyright file="PatientLifeGoalController.cs" company="Duly Health and Care">
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
using System.Linq;
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
    public class PatientLifeGoalController : DulyControllerBase
    {
        private const string DescriptionSavePatientLifeGoal = "Save PatientLifeGoal data";
        private const string DescriptionDeletePatientLifeGoal = "Delete patient LifeGoal";
        private const string DescriptionGetPatientLifeGoalById = "Get patient LifeGoal by id";
        private const string DescriptionPostPatientLifeGoalTargetMapping = "Get patient LifeGoal which has mapped with targets";
        private const string DescriptionGetPatientLifeGoalTargetMapping = "Get patient LifeGoal Target Mapping which has mapped with targets";
        private const string DescriptionGetPatientLifeGoalAndActionTracking = "Get Patient Actions Mapped by Life Goals";

        private readonly IPatientLifeGoalService _patientLifeGoalService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientLifeGoalController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="patientLifeGoalService">An instance of Patient Plan service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public PatientLifeGoalController(
              IPatientLifeGoalService patientLifeGoalService,
              ILogger<PatientLifeGoalController> logger,
              IWebHostEnvironment environment)
           : base(logger, environment)
        {
            _patientLifeGoalService = patientLifeGoalService;
        }

        [HttpPost("post-patient-life-goals")]
        [SwaggerOperation(
            Summary = nameof(PostOrUpdatePatientLifeGoal),
            Description = DescriptionSavePatientLifeGoal)]
        [SwaggerResponse(StatusCodes.Status202Accepted, DescriptionSavePatientLifeGoal, typeof(PostOrUpdatePatientLifeGoalResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<PostOrUpdatePatientLifeGoalResponse>> PostOrUpdatePatientLifeGoal(PostRequestForLifeGoals request)
        {
            try
            {
                var postPatientLifeGoalResponse = await _patientLifeGoalService.PostOrUpdateLifeGoalAsync(request);
                postPatientLifeGoalResponse.StatusCode = StatusCodes.Status202Accepted.ToString();
                postPatientLifeGoalResponse.Message = "Life goals updated successfully.";
                return StatusCode(StatusCodes.Status202Accepted, postPatientLifeGoalResponse);
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

        [HttpPost("delete-patient-life-goal")]
        [SwaggerOperation(
           Summary = nameof(DeletePatientLifeGoal),
           Description = DescriptionDeletePatientLifeGoal)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionDeletePatientLifeGoal, typeof(DeletePatientLifeGoalResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<DeletePatientLifeGoalResponse>>> DeletePatientLifeGoal([FromBody, SwaggerParameter("Patient LifeGoal Id")] List<long> patientLifeGoalIds)
        {
            try
            {
                List<DeletePatientLifeGoalResponse> response = new List<DeletePatientLifeGoalResponse>();
                foreach (var id in patientLifeGoalIds)
                {
                    var result = new DeletePatientLifeGoalResponse();
                    var patientLifeGoalId = await _patientLifeGoalService.DeletePatientLifeGoalAsync(id);
                    if (patientLifeGoalId != 0)
                    {
                        result.StatusCode = StatusCodes.Status200OK.ToString();
                        result.Message = "Life Goal deleted";
                    }
                    else
                    {
                        result.StatusCode = StatusCodes.Status204NoContent.ToString();
                        result.Message = "Patient Life Goal Id not found.";
                    }

                    response.Add(result);
                }

                return StatusCode(StatusCodes.Status200OK, response);
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

        [HttpGet("get-patient-life-goal-by-patientplanid/{patientPlanId}")]
        [SwaggerOperation(
        Summary = nameof(GetPatientLifeGoalById),
        Description = DescriptionGetPatientLifeGoalById)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionGetPatientLifeGoalById, typeof(GetPatientLifeGoalByPatientPlanIdResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<GetPatientLifeGoalByPatientPlanIdResponse>> GetPatientLifeGoalById([FromRoute, SwaggerParameter("Patient Plan Id")] long patientPlanId)
        {
            try
            {
                GetPatientLifeGoalByPatientPlanIdResponse response = new GetPatientLifeGoalByPatientPlanIdResponse();
                var getPatientLifeGoalById = await _patientLifeGoalService.GetPatientLifeGoalByPatientPlanIdAsync(patientPlanId);
                response.PatientLifeGoals = getPatientLifeGoalById;
                response.StatusCode = StatusCodes.Status200OK;
                return Ok(response);
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

        [HttpPost("post-patient-life-goals-target-mapping/{patientTargetId}")]
        [SwaggerOperation(
            Summary = nameof(PostPatientLifeGoalTargetMapping),
            Description = DescriptionPostPatientLifeGoalTargetMapping)]
        [SwaggerResponse(StatusCodes.Status201Created, DescriptionPostPatientLifeGoalTargetMapping, typeof(PostPatientLifeGoalTargetMappingResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<PostPatientLifeGoalTargetMappingResponse>> PostPatientLifeGoalTargetMapping(
            [Required, FromRoute, SwaggerParameter("Patient Target Id")] long patientTargetId,
            [Required, FromBody, SwaggerParameter("Patient Life Goal Ids")] IEnumerable<long> patientLifeGoalIds
        )
        {
            try
            {
                var result = new PostPatientLifeGoalTargetMappingResponse();
                var response = await _patientLifeGoalService.PostPatientLifeGoalTargetMappingAsync(patientTargetId, patientLifeGoalIds);
                result.Message = "Targets Mapped to Life Goals Successfully.";
                result.StatusCode = StatusCodes.Status201Created;
                return StatusCode(StatusCodes.Status201Created, result);
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

        [HttpGet("/care-plan/patient-life-goal-target-mapping/{patientTargetId}")]
        [SwaggerOperation(
            Summary = nameof(GetPatientLifeGoalTargetMappingByPatientTargetId),
            Description = DescriptionGetPatientLifeGoalTargetMapping)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionGetPatientLifeGoalTargetMapping, typeof(IEnumerable<GetPatientLifeGoalTargetMapping>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<GetPatientLifeGoalTargetMapping>>> GetPatientLifeGoalTargetMappingByPatientTargetId([Required, FromRoute, SwaggerParameter("Patient Target Id")] long patientTargetId)
        {
            try
            {
                var result = await _patientLifeGoalService.GetPatientLifeGoalTargetMappingByPatientIdAsync(patientTargetId);
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

        [HttpGet("/care-plan/patient-life-goal-and-action-tracking/{patientPlanId}")]
        [SwaggerOperation(
            Summary = nameof(GetPatientLifeGoalAndActionTracking),
            Description = DescriptionGetPatientLifeGoalAndActionTracking)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionGetPatientLifeGoalAndActionTracking, typeof(GetPatientLifeGoalAndActionTrackingResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<GetPatientLifeGoalAndActionTrackingResponse>> GetPatientLifeGoalAndActionTracking(
            [Required, FromRoute, SwaggerParameter("Patient Plan Id")] long patientPlanId
        )
        {
            try
            {
                var result = await _patientLifeGoalService.GetPatientLifeGoalAndActionTrackingAsync(patientPlanId);
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
