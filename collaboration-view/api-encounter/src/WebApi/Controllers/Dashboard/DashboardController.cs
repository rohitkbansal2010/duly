// -----------------------------------------------------------------------
// <copyright file="DashboardController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Contracts.Dashboard;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Annotations.Constants;
using Duly.Common.ApiExtensions.Constants;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces.Dashboard;
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

namespace Duly.CollaborationView.Encounter.Api.Controllers.Dashboard
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class DashboardController : DulyControllerBase
    {
        private const string GetPatientAndAppointmentCountDescription = "Returns the patient count and todays appointment for the particulat provider against location";
        private readonly IDashboardService _dashboardService;
        public DashboardController(
            IDashboardService dashboardSearvice,
            ILogger<DashboardController> logger,
            IWebHostEnvironment environment)
           : base(logger, environment)
        {
            _dashboardService = dashboardSearvice;
        }


        /// <summary>
        /// Returns <see cref="LabLocation"/> for a specific patient.
        /// </summary>
        /// <param name="lat">Latitude of a specific Location.</param>
        /// <param name="lng">Longitude of a specific Location.</param>
        /// <returns>Retrieved <see cref="LabLocation"/> instance.</returns>
        [HttpGet("GetPatientAndAppointmentCount")]
        [SwaggerOperation(
        Summary = nameof(GetPatientAndAppointmentCount),
        Description = GetPatientAndAppointmentCountDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns Lab Location by lat lng.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<List<PatientAndAppointmentCount>>> GetPatientAndAppointmentCount(string locationId)
        {
            var data = await _dashboardService.GetPatientAndAppointmentCountAsync(locationId);
            Validate(data);

            return Ok(data);
        }
    }
}
