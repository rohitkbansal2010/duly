// <copyright file="ReferralController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Annotations.Constants;
using Duly.Common.Annotations.Filters;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
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
    [Route("referral/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class ReferralController : DulyControllerBase
    {
        private const string DescriptionSaveReferral = "Save Referral Data";

        private readonly IReferralService _service;
        private readonly IScheduleSlotsservice _scheduleservice;
        private readonly IGetSlotsservice _getSlotsservice;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="service">An instance of Patient service.</param>
        /// <param name="scheduleservice">An instance of Schedule service.</param>
        /// <param name="getSlotsservice">An instance of Get Slot service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public ReferralController(
            IReferralService service,
            IScheduleSlotsservice scheduleservice,
            IGetSlotsservice getSlotsservice,
            ILogger<ReferralController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _service = service;
            _getSlotsservice = getSlotsservice;
            _scheduleservice = scheduleservice;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = nameof(PostReferral),
            Description = DescriptionSaveReferral)]
        [SwaggerResponse(StatusCodes.Status201Created, DescriptionSaveReferral, typeof(CreationResultResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [SwaggerOperationFilter(typeof(RequestBodyDescriptionFilter))]
        public async Task<ActionResult<ScheduleAppointmentResult>> PostReferral([FromBody] Contracts.ScheduleReferral referral)
        {
            try
            {
                var result = new CreationResultResponse();
                var record_Id = await _service.PostReferral(referral);
                result.RecordID = record_Id;
                result.CreationDate = System.DateTime.Now;
                result.StatusCode = StatusCodes.Status201Created.ToString();
                result.ErrorMessage = string.Empty;
                if (!referral.Skipped)
                {
                    DateTime dateTime = DateTime.ParseExact(referral.BookingSlot, "HH:mm:ss", CultureInfo.InvariantCulture);

                    var appointmentSchedulingRequest = new AppointmentSchedulingModel
                    {
                        ProviderId = referral.Provider_ID,
                        Date = referral.AptScheduleDate,
                        Time = dateTime.TimeOfDay,
                        LocationId = referral.Location_ID
                    };

                    var scheduledResult = await ScheduleReferralAppointmentForPatient(referral.Patient_ID, referral.Provider_ID, referral.Department_Id, referral.VisitTypeId, appointmentSchedulingRequest);
                    await _service.DataPostedToEpicAsync(record_Id);
                    return StatusCode(StatusCodes.Status201Created, scheduledResult);
                }
                else
                {
                    return StatusCode(StatusCodes.Status201Created, "Data Saved");
                }
            }
            catch (SqlException ex)
            {
                Logger.LogWarning(ex, ex.Message);
                return BadRequest(new FaultResponse { ErrorMessage = ex.Message });
            }
            catch (Exception e)
            {
                Logger.LogWarning(e, e.Message);
                return BadRequest(new FaultResponse { ErrorMessage = e.Message });
            }
        }

        private async Task<ActionResult<ScheduleAppointmentResult>> ScheduleReferralAppointmentForPatient(string patientId, string providerId, string departmentId, string visitTypeId, AppointmentSchedulingModel appointmentSchedulingRequest)
        {
            try
            {
                var data = await _scheduleservice.ScheduleReferralAppointmentForPatientAsync(appointmentSchedulingRequest, patientId, providerId, departmentId, visitTypeId);
                Validate(data);

                return data;
            }
            catch (Clinic.Api.Client.ApiException)
            {
                var existingTime = await IfTimeExists(appointmentSchedulingRequest, visitTypeId, departmentId, providerId);
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

        private async Task<bool> IfTimeExists(
            AppointmentSchedulingModel appointmentSchedulingRequest,
            string visitTypeId,
            string departmentId,
            string providerId)
        {
            var date = appointmentSchedulingRequest.Date.GetValueOrDefault();

            var data = await _getSlotsservice.GetReferralScheduleDateAsync(
                visitTypeId,
                departmentId,
                providerId,
                date,
                date);

            var existingTime = data
                .SelectMany(scheduleDate => scheduleDate.TimeSlots)
                .Any(slot => slot.Time == appointmentSchedulingRequest.Time.GetValueOrDefault());

            return existingTime;
        }
    }
}