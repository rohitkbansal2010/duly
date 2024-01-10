// <copyright file="SlotsController.cs" company="Duly Health and Care">
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
using System;
using System.Collections.Generic;
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
    public class SlotsController : DulyControllerBase
    {
        private const string DescriptionGetSlots = "Returns a list of schedule day with slots";
        private readonly IScheduleRepository _scheduleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SlotsController" /> class.
        /// </summary>
        /// <param name="scheduleRepository">A repository for working on <see cref="ScheduleDay"/>.</param>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public SlotsController(
            IScheduleRepository scheduleRepository,
            ILogger<SlotsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _scheduleRepository = scheduleRepository;
        }

        /// <summary>
        /// Returns list of <see cref="ScheduleDay"/> with slots.
        /// </summary>
        /// <param name="startDate">Start date of requested slots.</param>
        /// <param name="endDate">End date of requested slots.</param>
        /// <param name="providerId">Provider identifier.</param>
        /// <param name="departmentId">Department identifier.</param>
        /// <param name="visitTypeId">Visit type identifier.</param>
        /// <returns>List of <see cref="ScheduleDay"/>.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = nameof(GetSlots), Description = DescriptionGetSlots)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionGetSlots)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<ScheduleDay>>> GetSlots(
            [Required, FromQuery, SwaggerParameter("Start date"), SwaggerSchema(Format = "date")] DateTime startDate,
            [Required, FromQuery, SwaggerParameter("End date"), SwaggerSchema(Format = "date")] DateTime endDate,
            [Required, FromQuery, SwaggerParameter("Provider Id")] string providerId,
            [Required, FromQuery, SwaggerParameter("Department Id")] string departmentId,
            [Required, FromQuery, SwaggerParameter("Visit type Id")] string visitTypeId)
        {
            var data = await _scheduleRepository.GetScheduleAsync(startDate, endDate, providerId, departmentId, visitTypeId);

            Validate(data);

            return Ok(data);
        }
    }
}
