// -----------------------------------------------------------------------
// <copyright file="ActionPlansController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.Common.ApiExtensions.Constants;
using Duly.Common.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Controllers
{
    [Route(RoutePaths.DefaultPatientRoute)]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class ActionPlansController : ControllerBase
    {
        private static readonly Type CurrentType = typeof(ActionPlan);
        private readonly ILogger<ActionPlansController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionPlansController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        public ActionPlansController(
            ILogger<ActionPlansController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns ActionPlan by ID.
        /// </summary>
        /// <param name="id">Id of ActionPlan.</param>
        /// <param name="patientId">Id of Patient.</param>
        /// <returns>ActionPlan.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(nameof(GetActionPlanDataById), "Returns ActionPlan data")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns an acknowledge of execution result.", typeof(ActionPlan))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Returns a validation error result.", typeof(FaultResponse))]
        public async Task<ActionResult<ActionPlan>> GetActionPlanDataById(
            [Required, FromRoute, SwaggerParameter("ActionPlan Id")] string id,
            [Required, FromRoute, SwaggerParameter("Patient Id")] string patientId)
        {
            _logger.LogDebug("Request {CurrentType} {Id} for Patient {PatientId}", CurrentType.Name, id, patientId);

            return await Task.FromResult(new ActionPlan { Id = id });
        }

        /// <summary>
        /// Returns all available ActionPlans.
        /// </summary>
        /// <param name="patientId">Id of Patient.</param>
        /// <returns>Collection of ActionPlans.</returns>
        [HttpGet]
        [SwaggerOperation(nameof(GetActionPlans), "Returns all available ActionPlans")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns an acknowledge of execution result.", typeof(IEnumerable<ActionPlan>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Returns a validation error result.")]
        public async Task<ActionResult<IEnumerable<ActionPlan>>> GetActionPlans([Required, FromRoute, SwaggerParameter("Patient Id")] string patientId)
        {
            _logger.LogDebug("Request all {0}s for Patient {1}", CurrentType, patientId);

            var data = new List<ActionPlan>
                { new ActionPlan { Id = "1" }, new ActionPlan { Id = "2" }, new ActionPlan { Id = "3" } };

            return await Task.FromResult(data);
        }
    }
}
