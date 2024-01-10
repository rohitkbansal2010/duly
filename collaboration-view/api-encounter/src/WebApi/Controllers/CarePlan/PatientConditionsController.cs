// <copyright file="PatientConditionsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
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
using System.Collections.Generic;
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
    public class PatientConditionsController : DulyControllerBase
    {
        private const string DescriptionSavePatientConditions = "Save Patient Conditions data";
        private const string DescriptionGetConditionByPatientPlanId = "Get health conditions by id.";

        private readonly IPatientConditionsService _patientConditionsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientConditionsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="patientConditionsService">An instance of Patient Conditions service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public PatientConditionsController(
               IPatientConditionsService patientConditionsService,
               ILogger<PatientConditionsController> logger,
               IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _patientConditionsService = patientConditionsService;
        }

        [HttpPost("post-patient-conditions")]
        [SwaggerOperation(
            Summary = nameof(PostPatientConditions),
            Description = DescriptionSavePatientConditions)]
        [SwaggerResponse(StatusCodes.Status201Created, DescriptionSavePatientConditions, typeof(PostPatientConditionsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [SwaggerOperationFilter(typeof(RequestBodyDescriptionFilter))]
        public async Task<ActionResult<PostPatientConditionsResponse>> PostPatientConditions([FromBody] PatientConditions request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new FaultResponse { ErrorMessage = "Request body should not be empty." });
                }
                else if (request.PatientPlanId < 1)
                {
                    return BadRequest(new FaultResponse { ErrorMessage = "Enter a valid patient plan id." });
                }
                else
                {
                    var result = new PostPatientConditionsResponse();
                    await _patientConditionsService.PostPatientConditionsAsync(request);
                    result.Message = "Patient conditions updated successfully.";
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

        [HttpGet("get-condition-by-patientplanid/{patientPlanId}")]
        [SwaggerOperation(
            Summary = nameof(GetPatientConditionByPatientPlanId),
            Description = DescriptionGetConditionByPatientPlanId)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionGetConditionByPatientPlanId, typeof(IEnumerable<GetPatientConditionByPatientPlanIdResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<GetPatientConditionByPatientPlanIdResponse>> GetPatientConditionByPatientPlanId([FromRoute, SwaggerParameter("Patient Plan Id")] long patientPlanId)
        {
            try
            {
                if (patientPlanId < 1)
                {
                    return BadRequest(new FaultResponse { ErrorMessage = "Patient Plan ID should be greater than or equal to 1." });
                }
                else
                {
                    GetPatientConditionByPatientPlanIdResponse response = new GetPatientConditionByPatientPlanIdResponse();
                    var getConditionByPatientPlanId = await _patientConditionsService.GetConditionByPatientPlanId(patientPlanId);
                    response.Conditions = getConditionByPatientPlanId;
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
    }
}