// <copyright file="PatientPlanController.cs" company="Duly Health and Care">
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
    public class PatientPlanController : DulyControllerBase
    {
        private const string DescriptionSavePatientPlan = "Save PatientPlan data";
        private const string DescriptionUpdatePatientPlanStatus = "Update PatientPlan status";
        private const string DescriptionGetPatientPlanById = "Get PatientPlan data by patient plan id";
        private const string DescriptionUpdateFlourishStatement = "Update Flourish Statement by patient plan id";
        private const string DescriptionGetCount = "Get count for Patient Targets";

        private readonly IPatientPlanService _patientPlanService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientPlanController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="patientPlanService">An instance of Patient Plan service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public PatientPlanController(
               IPatientPlanService patientPlanService,
               ILogger<PatientPlanController> logger,
               IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _patientPlanService = patientPlanService;
        }

        [HttpPost("Post-Patient-Plan")]
        [SwaggerOperation(
            Summary = nameof(PostPatientPlan),
            Description = DescriptionSavePatientPlan)]
        [SwaggerResponse(StatusCodes.Status201Created, DescriptionSavePatientPlan, typeof(PostPatientPlanResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [SwaggerOperationFilter(typeof(RequestBodyDescriptionFilter))]
        [ProducesResponseType(typeof(PostPatientPlanResponse), StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PostPatientPlanResponse>> PostPatientPlan([FromBody] PatientPlan request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new FaultResponse { ErrorMessage = "Patient Plan Details are required." });
                }
                else
                {
                    var result = new PostPatientPlanResponse();

                    var patientPlanId = await _patientPlanService.PostPatientPlanAsync(request);
                    if (patientPlanId != 0)
                    {
                        result.PatientPlanId = patientPlanId;
                        result.StatusCode = StatusCodes.Status201Created;
                        result.Message = "Patient plan created successfully.";
                        return StatusCode(StatusCodes.Status201Created, result);
                    }
                    else
                    {
                        result.PatientPlanId = 0;
                        result.StatusCode = StatusCodes.Status409Conflict;
                        result.Message = "Plan for the given Patient ID already exists.";
                        return StatusCode(StatusCodes.Status409Conflict, result);
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

        [HttpPost("Update-Patient-Plan-Status-ById/{id}")]
        [SwaggerOperation(
           Summary = nameof(UpdatePatientPlanStatusById),
           Description = DescriptionUpdatePatientPlanStatus)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionUpdatePatientPlanStatus, typeof(UpdatePatientPlanStatusByIdResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [SwaggerOperationFilter(typeof(RequestBodyDescriptionFilter))]
        public async Task<ActionResult<UpdatePatientPlanStatusByIdResponse>> UpdatePatientPlanStatusById([FromRoute, SwaggerParameter("Patient Plan Id")] long id)
        {
            try
            {
                if (id < 1)
                {
                    return BadRequest(new FaultResponse { ErrorMessage = "Patient Plan ID should be greater than or equal to 1." });
                }
                else
                {
                    var result = new UpdatePatientPlanStatusByIdResponse();

                    var isCompleted = await _patientPlanService.UpdatePatientPlanStatusByIdAsync(id);
                    result.PatientPlanId = id;
                    result.IsCompleted = isCompleted;
                    result.Message = "Patient plan updated successfully.";
                    result.StatusCode = StatusCodes.Status200OK;

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

        [HttpGet("get-patient-plan-by-patientid/{patientId}")]
        [SwaggerOperation(
          Summary = nameof(GetPatientPlanByPatientId),
          Description = DescriptionGetPatientPlanById)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionGetPatientPlanById, typeof(GetPatientPlanByIdResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<GetPatientPlanByIdResponse>> GetPatientPlanByPatientId([FromRoute, SwaggerParameter("Patient Id")] string patientId)
        {
            try
            {
                if (patientId == null)
                {
                    return BadRequest(new FaultResponse { ErrorMessage = "Patient ID is required." });
                }
                else
                {
                    GetPatientPlanByIdResponse response = new GetPatientPlanByIdResponse();
                    var getPatientPlanByPatientId = await _patientPlanService.GetPatientPlanByPatientIdAsync(patientId);
                    response.PatientPlans = getPatientPlanByPatientId;
                    response.StatusCode = StatusCodes.Status200OK;
                    return Ok(response);
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

        [HttpPut]
        [SwaggerOperation(
        Summary = nameof(UpdateFlourishStatement),
        Description = DescriptionUpdateFlourishStatement)]
        [SwaggerResponse(StatusCodes.Status202Accepted, DescriptionUpdateFlourishStatement, typeof(UpdateFlourishStatementResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<UpdateFlourishStatementResponse>> UpdateFlourishStatement([FromBody] UpdateFlourishStatementRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new FaultResponse { ErrorMessage = "Request body is required." });
                }
                else
                {
                    var response = new UpdateFlourishStatementResponse();

                    var patientPlanId = await _patientPlanService.UpdateFlourishStatementAsync(request);
                    response.PatientPlanId = patientPlanId;
                    response.Message = "Flourish Statement Updated successfully.";
                    response.StatusCode = StatusCodes.Status202Accepted;

                    return StatusCode(StatusCodes.Status202Accepted, response);
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

        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(GetHealthPlanStats),
            Description = DescriptionGetCount)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionGetPatientPlanById, typeof(GetHealthPlanStats))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<GetHealthPlanStats>> GetHealthPlanStats([FromQuery, SwaggerParameter("Patient Plan Id")] long patientPlanId)
        {
            try
            {
                if (patientPlanId < 1)
                {
                    return BadRequest(new FaultResponse { ErrorMessage = "Enter a valid Patient Plan ID." });
                }
                else
                {
                    var count = await _patientPlanService.GetHealthPlanStatsByPatientPlanIdAsync(patientPlanId);

                    return Ok(count);
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
    }
}