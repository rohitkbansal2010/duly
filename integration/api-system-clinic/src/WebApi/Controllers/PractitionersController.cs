// -----------------------------------------------------------------------
// <copyright file="PractitionersController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Common.Annotations.Constants;
using Duly.Common.ApiExtensions.Constants;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
using Duly.Common.Security.Filters;
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

namespace Duly.Clinic.Api.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    public class PractitionersController : DulyControllerBase
    {
        private const string GetPractitionersForSiteDescription = "Returns Practitioners' general info for a specific site.";
        private const string GetPractitionersByIdentifiersDescription = "Returns Practitioners' general info for a set of identifiers.";

        private readonly IPractitionerGeneralInfoRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PractitionersController" /> class.
        /// </summary>
        /// <param name="repository">A repository for working on <see cref="PractitionerGeneralInfo"/>.</param>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public PractitionersController(
            IPractitionerGeneralInfoRepository repository,
            ILogger<PractitionersController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _repository = repository;
        }

        /// <summary>
        /// Returns all available items of <see cref="PractitionerGeneralInfo"/> for a site.
        /// </summary>
        /// <param name="siteId">Site Id.</param>
        /// <returns>Returns practitioners.</returns>
        [HttpGet(RoutePaths.DefaultSiteRoute)]
        [SwaggerOperation(
            Summary = nameof(GetPractitionersOfSite),
            Description = GetPractitionersForSiteDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns Practitioners.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [ObfuscatedIdentityParameters(new[] { "siteId" })]
        public async Task<ActionResult<IEnumerable<PractitionerGeneralInfo>>> GetPractitionersOfSite(
            [Required, FromRoute, SwaggerParameter("Site Id")] string siteId)
        {
            Logger.LogDebug("Request all Practitioners for Site '{SiteId}'", siteId);

            var data = await _repository.GetPractitionersBySiteIdAsync(siteId);

            Validate(data);

            return Ok(data);
        }

        /// <summary>
        /// Returns all available items of <see cref="PractitionerGeneralInfo"/> for a set of identifiers.
        /// </summary>
        /// <param name="identifiers">Identifiers of practitioners.</param>
        /// <returns>Returns practitioners.</returns>
        [HttpGet("[controller]")]
        [SwaggerOperation(
            Summary = nameof(GetPractitionersByIdentifiers),
            Description = GetPractitionersByIdentifiersDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns Practitioners.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<PractitionerGeneralInfo>>> GetPractitionersByIdentifiers(
            [Required, FromQuery, SwaggerParameter("identifiers")] string[] identifiers)
        {
            var data = await _repository.GetPractitionersByIdentifiersAsync(identifiers);

            Validate(data);

            return Ok(data);
        }
    }
}