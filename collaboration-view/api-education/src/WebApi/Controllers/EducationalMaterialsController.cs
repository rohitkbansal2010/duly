// -----------------------------------------------------------------------
// <copyright file="EducationalMaterialsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Shared.Contracts.Responses;
using Duly.Common.Annotations.Constants;
using Duly.Common.Infrastructure.Constants;
using Duly.Education.Api.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.Education.Api.Controllers
{
    [Route(RoutePaths.DefaultEducationRoute)]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class EducationalMaterialsController : ControllerBase
    {
        private const string GetEducationalMaterialDescription = "Returns an array of EducationalMaterial";

        private static readonly Type CurrentType = typeof(EducationalMaterial);

        private readonly ILogger<EducationalMaterialsController> _logger;
        private readonly IExamplesProvider<IEnumerable<EducationalMaterial>> _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EducationalMaterialsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="provider">Provides Examples for swagger.</param>
        public EducationalMaterialsController(
            ILogger<EducationalMaterialsController> logger,
            IExamplesProvider<IEnumerable<EducationalMaterial>> provider)
        {
            _logger = logger;
            _provider = provider;
        }

        /// <summary>
        /// Returns all available <see cref="EducationalMaterial"/>.
        /// </summary>
        /// <returns>An array of EducationalMaterial.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(GetEducationalMaterial),
            Description = GetEducationalMaterialDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetEducationalMaterialDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<EducationalMaterial>>> GetEducationalMaterial()
        {
            _logger.LogDebug("Request all {CurrentType}s", CurrentType.Name);

            var data = await Task.FromResult(_provider.GetExamples());

            return Ok(data);
        }
    }
}
