// <copyright file="AppointmentsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Annotations.Constants;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
using Duly.Ngdp.Api.Repositories.Interfaces;
using Duly.Ngdp.Contracts;
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

namespace Duly.Ngdp.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class AppointmentsController : DulyControllerBase
    {
        private const string GetAppointmentsForSpecificLocationDescription = "Returns an array of appointments for a specific location";
        private const string GetAppointmentsForSpecificCsnIdDescription = "Returns an appointment for a CSN Id";
        private const string GetAppointmentsForPatientByCsnIdDescription = "Returns an array of appointments for the patient which has an appointment with specific CSN Id";
        private const string GetReferralAppointmentsByReferralIdDescription = "Returns referral appointments by referral Id";

        private readonly IAppointmentRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        /// <param name="repository">A repository for working on <see cref="Appointment"/>.</param>
        public AppointmentsController(
            ILogger<AppointmentsController> logger,
            IWebHostEnvironment environment,
            IAppointmentRepository repository)
            : base(logger, environment)
        {
            _repository = repository;
        }

        /// <summary>
        /// Returns all available <see cref="Appointment"/> for a specific location.
        /// </summary>
        /// <param name="locationId">Location Id.</param>
        /// <param name="startDate">Filter by start date.</param>
        /// <param name="endDate">Filter by end date.</param>
        /// <param name="includedVisitTypes">Visit type Ids that should be included.</param>
        /// <param name="excludedAppointmentStatuses">Appointment statuses that should be excluded, if null or empty includes all.</param>
        /// <returns>An array of appointments for specific location.</returns>
        [HttpGet("locations/{locationId}/[controller]")]
        [SwaggerOperation(
            Summary = nameof(GetAppointmentsForSpecificLocation),
            Description = GetAppointmentsForSpecificLocationDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetAppointmentsForSpecificLocationDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsForSpecificLocation(
            [Required, FromRoute, SwaggerParameter("Location Id")] string locationId,
            [Required, FromQuery, SwaggerParameter("Filter by start date")] DateTimeOffset startDate,
            [Required, FromQuery, SwaggerParameter("Filter by end date")] DateTimeOffset endDate,
            [Required, FromQuery, SwaggerParameter("Visit type Ids that should be included")] string[] includedVisitTypes,
            [FromQuery, SwaggerParameter("Appointment statuses that should be excluded, if null or empty includes all")] AppointmentStatusParam[] excludedAppointmentStatuses = null)
        {
            var data = await _repository.GetAppointmentsForLocationByDateRangeAsync(locationId, startDate, endDate, includedVisitTypes, excludedAppointmentStatuses);

            Validate(data);

            return Ok(data);
        }

        /// <summary>
        /// Returns an <see cref="Appointment"/> for a specific CSN Id.
        /// </summary>
        /// <param name="csnId">Appointment CSN Id.</param>
        /// <returns>An appointment for specific CSN Id.</returns>
        [HttpGet("[controller]/{csnId}")]
        [SwaggerOperation(
            Summary = nameof(GetAppointmentByCsnId),
            Description = GetAppointmentsForSpecificCsnIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetAppointmentsForSpecificCsnIdDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<Appointment>> GetAppointmentByCsnId(
            [Required, FromRoute, SwaggerParameter("CSN Id")] string csnId)
        {
            var data = await _repository.GetAppointmentByCsnId(csnId);

            Validate(data);

            return Ok(data);
        }

        /// <summary>
        /// Returns all available <see cref="Appointment"/> for the patient which has an appointment with specific CSN Id.
        /// </summary>
        /// <param name="csnId">Appointment CSN Id.</param>
        /// <param name="startDate">Filter by start date.</param>
        /// <param name="endDate">Filter by end date.</param>
        /// <param name="includedAppointmentStatuses">Appointment statuses that should be included.</param>
        /// <returns>An array of appointments.</returns>
        [HttpGet("[controller]/{csnId}/forSamePatient")]
        [SwaggerOperation(
            Summary = nameof(GetAppointmentsForPatientByCsnId),
            Description = GetAppointmentsForPatientByCsnIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetAppointmentsForPatientByCsnIdDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsForPatientByCsnId(
            [Required, FromRoute, SwaggerParameter("CSN Id")] string csnId,
            [Required, FromQuery, SwaggerParameter("Filter by start date")] DateTimeOffset startDate,
            [Required, FromQuery, SwaggerParameter("Filter by end date")] DateTimeOffset endDate,
            [Required, FromQuery, SwaggerParameter("Appointment statuses that should be included")] AppointmentStatusParam[] includedAppointmentStatuses)
        {
            var data = await _repository.GetAppointmentsForPatientByCsnIdAsync(csnId, startDate, endDate, includedAppointmentStatuses);

            Validate(data);

            return Ok(data);
        }

        /// <summary>
        /// Returns all available <see cref="ReferralAppointment"/> which match with specific referral Id.
        /// </summary>
        /// <param name="referralId">Referral Id.</param>
        /// <returns>A collection of referral appointments.</returns>
        [HttpGet("[controller]/referrals/{referralId}")]
        [SwaggerOperation(
            Summary = nameof(GetReferralAppointmentsByReferralId),
            Description = GetReferralAppointmentsByReferralIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetReferralAppointmentsByReferralIdDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<ReferralAppointment>>> GetReferralAppointmentsByReferralId(
            [Required, FromRoute, SwaggerParameter("Referral Id")] string referralId)
        {
            var data = await _repository.GetReferralAppointmentsByReferralIdAsync(referralId);

            Validate(data);

            return Ok(data);
        }
    }
}
