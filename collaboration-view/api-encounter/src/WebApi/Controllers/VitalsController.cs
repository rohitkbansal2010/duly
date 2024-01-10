// -----------------------------------------------------------------------
// <copyright file="VitalsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
    /// <summary>
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-363.
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-1080.
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-1299.
    /// </summary>
    [Route(RoutePaths.DefaultPatientRoute)]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class VitalsController : DulyControllerBase
    {
        private const string GetVitalsCardsDescription = "Returns all available Vitals cards of a specific patient for today.";
        private const string GetChartByVitalsCardTypeDescription = "Returns a historical data chart for all requested vital signs for a specific patient.";

        private readonly IVitalService _vitalService;

        /// <summary>
        /// Initializes a new instance of the <see cref="VitalsController" /> class.
        /// </summary>
        /// <param name="vitalService">An instance of Vitals service.</param>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public VitalsController(
            IVitalService vitalService,
            ILogger<VitalsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _vitalService = vitalService;
        }

        /// <summary>
        /// Returns all available Vitals cards of a specific patient for today.
        /// </summary>
        /// <param name="patientId">Patient id.</param>
        /// <returns>Array of vitals cards for today.</returns>
        [HttpGet("Cards")]
        [SwaggerOperation(
            Summary = nameof(GetVitalsCards),
            Description = GetVitalsCardsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetVitalsCardsDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<VitalsCard>>> GetVitalsCards(
            [Required, FromRoute, SwaggerParameter("Patient Id")] string patientId)
        {
            var cards = await _vitalService.GetLatestVitalsForPatientAsync(patientId);

            Validate(cards);

            return Ok(cards);
        }

        /// <summary>
        /// Returns a historical data chart for all requested vital signs for a specific patient.
        /// </summary>
        /// <param name="patientId">Patient id.</param>
        /// <param name="vitalsCardType">Type of vitals card.</param>
        /// <returns>Chart of vitals.</returns>
        [HttpGet("Chart")]
        [SwaggerOperation(
            Summary = nameof(GetChartByVitalsCardType),
            Description = GetChartByVitalsCardTypeDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetChartByVitalsCardTypeDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<VitalHistory>> GetChartByVitalsCardType(
            [Required, FromRoute, SwaggerParameter("Patient Id")] string patientId,
            [Required, FromQuery, SwaggerParameter("Vitals card type")] VitalsCardType vitalsCardType)
        {
            var vitalHistory = await _vitalService.GetVitalHistoryForPatientByVitalsCardType(patientId, DateTime.Now.Date, vitalsCardType);
            if (vitalHistory == null)
            {
                throw new EntityNotFoundException(nameof(vitalHistory), patientId);
            }

            Validate(vitalHistory);

            return Ok(vitalHistory);
        }
    }
}
