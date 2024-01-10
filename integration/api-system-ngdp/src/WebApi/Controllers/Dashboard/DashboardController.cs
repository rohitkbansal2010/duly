using Duly.Common.Annotations.Constants;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
using Duly.Ngdp.Api.Repositories.Interfaces;
using Duly.Ngdp.Api.Repositories.Interfaces.Dashboard;
using Duly.Ngdp.Contracts;
using Duly.Ngdp.Contracts.Dashboard;
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

namespace Duly.Ngdp.Api.Controllers.Dashboard
{
    [ApiController]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class DashboardController : DulyControllerBase
    {
        private const string GetPatientAndAppointmentCountDescription = "Returns the patient count and todays appointment for the particulat provider against location";
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardController(
            IDashboardRepository dashboardRepository,
            ILogger<DashboardController> logger,
            IWebHostEnvironment environment)
           : base(logger, environment)
        {
            _dashboardRepository = dashboardRepository;
        }


        /// <summary>
        /// Returns <see cref="PatientAndAppointmentCount"/> for a specific patient.
        /// </summary>
        /// <param name="locationId">Location.</param>
        /// <returns>Retrieved <see cref="PatientAndAppointmentCount"/> instance.</returns>
        [HttpGet("GetPatientAndAppointmentCount")]
        [SwaggerOperation(
        Summary = nameof(GetPatientAndAppointmentCount),
        Description = GetPatientAndAppointmentCountDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns Lab Location by lat lng.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<PatientAndAppointmentCount>>> GetPatientAndAppointmentCount(string locationId)
        {
            var data = await _dashboardRepository.GetPatientAndAppointmentCountAsync(locationId);
            Validate(data);

            return Ok(data);
        }
    }
}
