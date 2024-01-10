// <copyright file="AppointmentsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Common.Annotations.Constants;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    public class AppointmentsController : DulyControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentsController" /> class.
        /// </summary>
        /// <param name="appointmentRepository">A repository for working with appointment.</param>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public AppointmentsController(
            IAppointmentRepository appointmentRepository,
            ILogger<AppointmentsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _appointmentRepository = appointmentRepository;
        }

        /// <summary>
        /// Returns <see cref="ScheduledAppointment"/>.
        /// </summary>
        /// <param name="model">An instance of <see cref="ScheduleAppointmentModel"/>.</param>
        /// <returns>Created appointment of <see cref="ScheduledAppointment"/>.</returns>
        [HttpPost]
        [SwaggerOperation(
            Summary = nameof(ScheduleAppointment),
            Description = "Schedules new appointment and returns it")]
        [SwaggerResponse(StatusCodes.Status200OK, "Scheduled appointment")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        public async Task<ActionResult<ScheduledAppointment>> ScheduleAppointment(
            [Required, FromBody, SwaggerParameter("Schedule appointment model")] ScheduleAppointmentModel model)
        {
            var data = await _appointmentRepository.ScheduleAppointmentAsync(model);

            Validate(data);

            return Ok(data);
        }
    }
}
