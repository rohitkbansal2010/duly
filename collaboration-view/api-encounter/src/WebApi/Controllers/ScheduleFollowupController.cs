// <copyright file="ScheduleFollowupController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Annotations.Constants;
using Duly.Common.Annotations.Filters;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
using Duly.Common.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Controllers
{
    [Route("schedulefollowup/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class ScheduleFollowupController : DulyControllerBase
    {
        private const string DescriptionSaveScheduleFollowup = "Save Schedule followup Data";

        private readonly IScheduleFollowupService _service;
        private readonly IScheduleSlotsservice _scheduleservice;
        private readonly IGetSlotsservice _getSlotsservice;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleFollowupController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="service">An instance of Post follow Up service.</param>
        /// <param name="scheduleservice">An instance of Schedule service.</param>
        /// <param name="getSlotsservice">An instance of Get Slot service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public ScheduleFollowupController(
            IScheduleFollowupService service,
            IScheduleSlotsservice scheduleservice,
            IGetSlotsservice getSlotsservice,
            ILogger<ScheduleFollowupController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _service = service;
            _scheduleservice = scheduleservice;
            _getSlotsservice = getSlotsservice;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = nameof(PostScheduleFollowUp),
            Description = DescriptionSaveScheduleFollowup)]
        [SwaggerResponse(StatusCodes.Status201Created, DescriptionSaveScheduleFollowup, typeof(CreationResultResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [SwaggerOperationFilter(typeof(RequestBodyDescriptionFilter))]
        public async Task<ActionResult<ScheduleAppointmentResult>> PostScheduleFollowUp([FromBody] Contracts.ScheduleFollowUp scheduleFollowUp)
        {
            var result = new CreationResultResponse();
            var DataSaved = "Data Saved";

            try
            {
                var record_ID = await _service.PostScheduleFollowup(scheduleFollowUp);
                result.RecordID = record_ID;
                result.CreationDate = System.DateTime.Now;
                result.StatusCode = StatusCodes.Status201Created.ToString();
                result.ErrorMessage = string.Empty;

                if (!scheduleFollowUp.Skipped)
                {
                    DateTime dateTime = DateTime.ParseExact(scheduleFollowUp.BookingSlot, "HH:mm:ss", CultureInfo.InvariantCulture);

                    var appointmentSchedulingRequest = new AppointmentSchedulingModel
                    {
                        ProviderId = scheduleFollowUp.Provider_ID,
                        Date = scheduleFollowUp.AptScheduleDate,
                        Time = dateTime.TimeOfDay,
                        LocationId = scheduleFollowUp.Location_ID,
                        VisitTypeId = scheduleFollowUp.VisitTypeId
                    };

                    var scheduledResult = await ScheduleAppointmentForPatient(scheduleFollowUp.Appointment_Id, appointmentSchedulingRequest);
                    await _service.DataPostedToEpicAsync(record_ID);
                    return StatusCode(StatusCodes.Status201Created, scheduledResult);
                }
                else
                {
                    return StatusCode(StatusCodes.Status201Created, DataSaved);
                }
            }
            catch (SqlException ex)
            {
                Logger.LogWarning(ex, ex.Message);
                return BadRequest(new FaultResponse { ErrorMessage = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                Logger.LogWarning(ex, ex.Message);
                return NotFound(new FaultResponse { ErrorMessage = ex.Message });
            }
            catch (Exception e)
            {
                Logger.LogWarning(e, e.Message);
                return BadRequest(new FaultResponse { ErrorMessage = e.Message });
            }
        }

        private async Task<ActionResult<ScheduleAppointmentResult>> ScheduleAppointmentForPatient(string appointmentId, AppointmentSchedulingModel appointmentSchedulingRequest)
        {
            try
            {
                var data = await _scheduleservice.ScheduleAppointmentForPatientAsync(appointmentSchedulingRequest, appointmentId);
                Validate(data);

                return data;
            }
            catch (Clinic.Api.Client.ApiException)
            {
                var existingTime = await IfTimeExists(appointmentSchedulingRequest, appointmentId);
                if (!existingTime)
                {
                    return Conflict(new AppointmentSchedulingFault
                    {
                        ErrorMessage =
                    "Appointment scheduling failed because requested time slot has already been taken.",
                        Reason = AppointmentSchedulingFaultReason.TimeSlotNotAvailable
                    });
                }

                throw;
            }
        }

        private async Task<bool> IfTimeExists(AppointmentSchedulingModel appointmentSchedulingRequest, string appointmentId)
        {
            var date = appointmentSchedulingRequest.Date.GetValueOrDefault();

            var data = await _getSlotsservice.GetScheduleDateAsync(
                appointmentSchedulingRequest.VisitTypeId,
                appointmentId,
                date,
                date);

            var existingTime = data
                .SelectMany(scheduleDate => scheduleDate.TimeSlots)
                .Any(slot => slot.Time == appointmentSchedulingRequest.Time.GetValueOrDefault());

            return existingTime;
        }
    }
}