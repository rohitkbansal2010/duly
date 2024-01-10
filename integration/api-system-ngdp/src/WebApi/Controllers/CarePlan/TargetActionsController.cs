// <copyright file="TargetActionsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Annotations.Constants;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
using Duly.Ngdp.Api.Repositories.Interfaces.CarePlan;
using Duly.Ngdp.Contracts.CarePlan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Controllers.CarePlan
{
    [ApiController]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class TargetActionsController : DulyControllerBase
    {
        private const string TargetActionsDescription = "Get target actions by target id.";

        private readonly ITargetActionsRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetActionsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        /// <param name="repository">A repository for working with <see cref="ConditionTargets"/>.</param>
        public TargetActionsController(
            ILogger<ConditionTargetsController> logger,
            IWebHostEnvironment environment,
            ITargetActionsRepository repository)
            : base(logger, environment)
        {
            _repository = repository;
        }

        [HttpGet("[controller]/{targetId}")]
        [SwaggerOperation(
           Summary = nameof(GetActionsForTarget),
           Description = TargetActionsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, TargetActionsDescription, typeof(IEnumerable<TargetActions>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<TargetActions>>> GetActionsForTarget([Required, FromRoute, SwaggerParameter("Target Id")] long targetId)
        {
            var result = await _repository.GetTargetActionsByTargetIdAsync(targetId);
            return Ok(result);
        }
    }
}