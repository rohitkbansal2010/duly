// -----------------------------------------------------------------------
// <copyright file="SitesController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
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

namespace Duly.CollaborationView.Encounter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class SitesController : DulyControllerBase
    {
        private const string GetSitesDescription = "Returns an array of Sites of the server";
        private const string GetSiteByIdDescription = "Return Site by Id";

        private readonly ISiteService _siteService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitesController"/> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="siteService">An instance of Site service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public SitesController(
            ISiteService siteService,
            ILogger<SitesController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _siteService = siteService;
        }

        /// <summary>
        /// Returns all available <see cref="Site"/> of the server.
        /// </summary>
        /// <returns>Returns sites of the server.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(GetSites),
            Description = GetSitesDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns sites of the server.")]

        public async Task<ActionResult<IEnumerable<Site>>> GetSites()
        {
            var data = await _siteService.GetSitesAsync();

            Validate(data);

            return Ok(data);
        }

        /// <summary>
        /// Returns <see cref="Site"/> by ID.
        /// </summary>
        /// <param name="siteId">Identifier of the <see cref="Site"/>.</param>
        /// <returns>Returns sites of the server.</returns>
        [HttpGet("{siteId}")]
        [SwaggerOperation(
            Summary = nameof(GetSiteById),
            Description = GetSiteByIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns site.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]

        public async Task<ActionResult<Site>> GetSiteById(
            [Required, FromRoute, SwaggerParameter("Site Id")] string siteId)
        {
            var data = await _siteService.GetSiteAsync(siteId);

            if (data == null)
            {
                throw new EntityNotFoundException(nameof(Site), siteId);
            }

            Validate(data);

            return Ok(data);
        }
    }
}
