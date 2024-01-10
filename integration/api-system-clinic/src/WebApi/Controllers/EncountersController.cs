// <copyright file="EncountersController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Common.ApiExtensions.Constants;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Security.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Controllers
{
    [Route(RoutePaths.DefaultSiteRoute)]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    public class EncountersController : DulyControllerBase
    {
        private const string GetEncountersForADayDescription = "Returns an array of Encounters for the date";
        private readonly IEncounterRepository _encounterRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EncountersController" /> class.
        /// </summary>
        /// <param name="encounterRepository">A repository for working on <see cref="Encounter"/>.</param>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public EncountersController(
            IEncounterRepository encounterRepository,
            ILogger<EncountersController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _encounterRepository = encounterRepository;
        }

        /// <summary>
        /// Returns all available items of <see cref="Encounter"/> for a day.
        /// </summary>
        /// <param name="siteId">Site Id.</param>
        /// <param name="date">Calendar Date.</param>
        /// <returns>Returns appointments for a specific date.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(GetEncountersOfSiteByDate),
            Description = GetEncountersForADayDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns encounters for a specific date.")]
        [ObfuscatedIdentityParameters(new[] { "siteId" })]
        public async Task<ActionResult<IEnumerable<Encounter>>> GetEncountersOfSiteByDate(
            [Required, FromRoute, SwaggerParameter("Site Id")] string siteId,
            [Required, FromQuery, SwaggerParameter("The day for which you need a calendar, Format:yyyy-MM-dd"), SwaggerSchema(Format = "date")] DateTime date)
        {
            Logger.LogDebug("Request all Encounters on {Date} for Site '{SiteId}'", date, siteId);

            var data = await _encounterRepository.GetEncountersForSiteByDateAsync(siteId, date);

            Validate(data);

            return Ok(data);
        }
    }
}
