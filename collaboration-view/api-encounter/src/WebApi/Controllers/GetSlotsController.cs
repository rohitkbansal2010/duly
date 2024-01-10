// <copyright file="GetSlotsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Annotations.Constants;
using Duly.Common.ApiExtensions.Constants;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
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
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-1286.
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-1096.
    /// </summary>
    //[Route("{appointmentId}/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class GetSlotsController : DulyControllerBase
    {
        private const string GetOpenTimeSlotsForProviderDescription = "Returns available time slots grouped by date for provided token, provider and location. " +
                                                                                   "Dates and time slots within each date are sorted in ascending order. " +
                                                                                   "All times are in the local timezone for the requested location.";

        private const string GetOpenReferralTimeSlotsForProviderDescription = "Returns available time slots grouped by date for provided token, provider and location. " +
                                                                                   "Dates and time slots within each date are sorted in ascending order. " +
                                                                                   "All times are in the local timezone for the requested location.";

        private const string GetImagingTimeSlotsDescription = "Returns available time slots grouped by date for provided token, provider and location. " +
                                                                                   "Dates and time slots within each date are sorted in ascending order. " +
                                                                                   "All times are in the local timezone for the requested location.";
        private readonly IGetSlotsservice _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSlotsController" /> class.
        /// </summary>
        /// <param name="service">An instance of get slots.</param>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public GetSlotsController(
            IGetSlotsservice service,
            ILogger<GetSlotsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _service = service;
        }

        [HttpGet("{visitTypeId}/appointmentId/{appointmentId}/TimeSlotsOpen")]
        [SwaggerOperation(
            Summary = nameof(GetOpenTimeSlotsForProvider),
            Description = GetOpenTimeSlotsForProviderDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetOpenTimeSlotsForProviderDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<ScheduleDate>>> GetOpenTimeSlotsForProvider(
            [Required, FromRoute] string visitTypeId,
            [Required, FromRoute] string appointmentId,
            [Required, FromQuery, SwaggerSchema(Format = "date"), SwaggerParameter("Time slots search range start date.")] DateTime startDate,
            [Required, FromQuery, SwaggerSchema(Format = "date"), SwaggerParameter("Time slots search range end date.")] DateTime endDate)
        {
            var data = await _service.GetScheduleDateAsync(visitTypeId, appointmentId, startDate, endDate);

            Validate(data);

            return Ok(data);
        }

        [HttpGet("{visitTypeId}/departmentId/{departmentId}/providerId/{providerId}/TimeSlotsOpen")]
        [SwaggerOperation(
            Summary = nameof(GetOpenReferralTimeSlotsForProvider),
            Description = GetOpenReferralTimeSlotsForProviderDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetOpenReferralTimeSlotsForProviderDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<ScheduleDate>>> GetOpenReferralTimeSlotsForProvider(
            [Required, FromRoute] string visitTypeId,
            [Required, FromRoute] string departmentId,
            [Required, FromRoute] string providerId,
            [Required, FromQuery, SwaggerSchema(Format = "date"), SwaggerParameter("Time slots search range start date.")] DateTime startDate,
            [Required, FromQuery, SwaggerSchema(Format = "date"), SwaggerParameter("Time slots search range end date.")] DateTime endDate)
        {
            var data = await _service.GetReferralScheduleDateAsync(visitTypeId, departmentId, providerId, startDate, endDate);

            Validate(data);

            return Ok(data);
        }

        [HttpPost("ImagingTimeSlots")]
        [SwaggerOperation(
            Summary = nameof(ImagingTimeSlots),
            Description = GetImagingTimeSlotsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetImagingTimeSlotsDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, typeof(FaultResponse))]
        public async Task<ActionResult<List<ImagingScheduleDate>>> ImagingTimeSlots([Required, FromBody] ImagingTimeSlot imagingSlot)
        {
            var data = await _service.GetImagingScheduleAsync(imagingSlot);

            Validate(data);

            return Ok(data);
        }
    }
}
