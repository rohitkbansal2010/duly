// <copyright file="CustomTargetsController.cs" company="Duly Health and Care">
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
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Controllers.CarePlan
{
    [ApiController]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class CustomTargetsController : DulyControllerBase
    {
        private const string CreateCustomTargetsDescription = "Save custom Targets of the Conditions.";

        private readonly ICustomTargetsRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTargetsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        /// <param name="repository">A repository for working with <see cref=""/>.</param>
        public CustomTargetsController(
                   ICustomTargetsRepository repository,
                   ILogger<CustomTargetsController> logger,
                   IWebHostEnvironment environment)
                   : base(logger, environment)
        {
            _repository = repository;
        }

        [HttpPost("[controller]")]
        [SwaggerOperation(
             Summary = nameof(PostCustomTargets),
             Description = CreateCustomTargetsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, CreateCustomTargetsDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<int>> PostCustomTargets([FromBody] CustomTargets request)
        {
            var data = await _repository.PostCustomTargetsAsync(request);

            return Ok(data);
        }
    }
}
