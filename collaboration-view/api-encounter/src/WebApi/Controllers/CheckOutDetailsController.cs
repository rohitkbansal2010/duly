// <copyright file="CheckOutDetailsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Annotations.Constants;
using Duly.Common.ApiExtensions.Constants;
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
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Controllers
{
    /// <summary>
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-1286.
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-1096.
    /// </summary>
    [Route("{appointmentId}/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class CheckOutDetailsController : DulyControllerBase
    {
        private const string GetCheckOutDetailsDescription = "Returns an information about Lab Details, Imaging, FollowUp, ScheduleReferral.";

        private readonly ICheckOutDetailsservice _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckOutDetailsController" /> class.
        /// </summary>
        /// <param name="service">An instance of ImmunizationService service.</param>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public CheckOutDetailsController(
            ICheckOutDetailsservice service,
            ILogger<CheckOutDetailsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _service = service;
        }

        /// <summary>
        /// Returns <see cref="CheckOutDetails"/> that represents an information about a patient's immunization,
        /// including information on recommended and past immunizations, as well as the patient's immunization progress.
        /// </summary>
        /// <param name="appointmentId">Id of a specific patient.</param>
        /// <returns>An instance of <see cref="CheckOutDetails"/> for a specific patient.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(GetCheckOutDetails),
            Description = GetCheckOutDetailsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetCheckOutDetailsDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<CheckOutDetails>> GetCheckOutDetails(string appointmentId)
        {
            try
            {
                var data = await _service.GetCheckOutDetailsAsync(appointmentId);

                Validate(data);

                return Ok(data);
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
