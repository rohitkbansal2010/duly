// -----------------------------------------------------------------------
// <copyright file="ConfigurationsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Resource.Api.Contracts;
using Duly.CollaborationView.Resource.Api.Repositories.Interfaces;
using Duly.Common.Annotations.Constants;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
using Duly.Common.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Resource.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class ConfigurationsController : DulyControllerBase
    {
        private const string GetConfigurationsDescription = "Returns all available configurations for the given parameters.";

        private readonly IConfigurationRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="repository">An instance of Configuration repository.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public ConfigurationsController(
            IConfigurationRepository repository,
            ILogger<ConfigurationsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _repository = repository;
        }

        /// <summary>
        /// An array of available configurations that can includes the following types:
        /// <see cref="NavigationModulesUiConfiguration"/>.
        /// </summary>
        /// <param name="appPart">An application part of the defined in <see cref="UiConfigurationTargetAreaType"/>.</param>
        /// <param name="siteId">Id of a specific site.</param>
        /// <param name="patientId">Id of a specific Patient.</param>
        /// <param name="targetAreaType">A UI configuration target area type of the defined in <see cref="UiConfigurationTargetAreaType"/>.</param>
        /// <returns>An array of available configurations.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(GetConfigurations),
            Description = GetConfigurationsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetConfigurationsDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<UiConfiguration>>> GetConfigurations(
            [Required, FromQuery, SwaggerParameter("Application part")] ApplicationPart appPart,
            [FromQuery, SwaggerParameter("Site Id")] string siteId,
            [FromQuery, SwaggerParameter("Patient Id")] string patientId,
            [FromQuery, SwaggerParameter("UI configuration target area type")] UiConfigurationTargetAreaType? targetAreaType)
        {
            // TODO: move to a better place
            CalculateConfigType(siteId, patientId);

            var result = await _repository.GetConfigurationsAsync(appPart, siteId, patientId, targetAreaType);

            Validate(result);

            return Ok(result);
        }

        private static UiConfigurationType CalculateConfigType(string siteId, string patientId)
        {
            if (string.IsNullOrWhiteSpace(siteId) && string.IsNullOrWhiteSpace(patientId))
            {
                return UiConfigurationType.GlobalConfiguration;
            }

            if (string.IsNullOrWhiteSpace(patientId))
            {
                return UiConfigurationType.ClinicConfiguration;
            }

            if (!string.IsNullOrWhiteSpace(siteId))
            {
                return UiConfigurationType.PatientConfiguration;
            }

            throw new BadDataException($"The {nameof(siteId)} parameter cannot be null or empty if the {nameof(patientId)} parameter is specified.");
        }
    }
}
