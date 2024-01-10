// -----------------------------------------------------------------------
// <copyright file="AppointmentsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Contracts.MockData;
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
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Controllers
{
    /// <summary>
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-399.
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-402.
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-1496.
    /// </summary>
    [Route(RoutePaths.DefaultSiteRoute)]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class AppointmentsController : DulyControllerBase
    {
        private const string GetAppointmentsForDateRangeDescription = "Returns a collection of Appointments for the date range";
        private const string GetAppointmentsForSamePatientByAppointmentIdDescription = "Returns an information about a grouped schedule appointments for the patient which has an appointment with specific appointment Id.";
        private const string GetScheduleFollowUpOrderByPatientIdDescription = "Returns the collection of schedule followUp by patient id.";
        private const string GetLabandImagingLocationsDescription = "Returns the collection of Lab and Imaging by patient id.";
        private const string GetReferralDetailsByPatientIdDescription = "Returns the collection of Appointment Referral Details by Patient Id.";

        private readonly IAppointmentService _appointmentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="appointmentService">An instance of Appointment service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public AppointmentsController(
            IAppointmentService appointmentService,
            ILogger<AppointmentsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _appointmentService = appointmentService;
        }

        /// <summary>
        /// Returns all available <see cref="Appointment"/> for a specific site filtered by a date range.
        /// </summary>
        /// <param name="siteId">Id of a specific site.</param>
        /// <param name="startDate">Filter by start date.</param>
        /// <param name="endDate">Filter by end date.</param>
        /// <returns>Returns appointments for a specific site filtered by a date range.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(GetAppointmentsForSiteByDateRange),
            Description = GetAppointmentsForDateRangeDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns appointments for a specific site filtered by a date range.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(string))]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsForSiteByDateRange(
            [Required, FromRoute, SwaggerParameter("Site Id")] string siteId,
            [Required, FromQuery, SwaggerParameter("Filter by start date")] DateTimeOffset startDate,
            [Required, FromQuery, SwaggerParameter("Filter by end date")] DateTimeOffset endDate)
        {
            var data = await _appointmentService.GetAppointmentsBySiteIdAndDateRangeAsync(siteId, startDate, endDate);

            Validate(data);

            return Ok(data);
        }

        /// <summary>
        /// Returns <see cref="PatientAppointments"/> that represents an information about a grouped schedule appointments
        /// for the patient which has an appointment with specific appointment Id.
        /// </summary>
        /// <param name="appointmentId">Id of a specific appointment.</param>
        /// <returns>An instance of <see cref="PatientAppointments"/> for the same patient.</returns>
        [HttpGet("/[controller]/{appointmentId}/forSamePatient")]
        [SwaggerOperation(
            Summary = nameof(GetAppointmentsForSamePatientByAppointmentId),
            Description = GetAppointmentsForSamePatientByAppointmentIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetAppointmentsForSamePatientByAppointmentIdDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<PatientAppointments>> GetAppointmentsForSamePatientByAppointmentId(
            [Required, FromRoute, SwaggerParameter("Appointment Id")] string appointmentId)
        {
            var patientAppointments = await _appointmentService.GetAppointmentsForSamePatientByAppointmentIdAsync(appointmentId);

            Validate(patientAppointments);

            return Ok(patientAppointments);
        }

        /// <summary>
        /// Returns hard coded data.
        /// </summary>
        /// <param name="patientId">Id of ScheduleFollowUpOrder.</param>
        /// <returns>Returns hard coded json data.</returns>
        [HttpGet("/v1/appointment/{patientId}/follow-up-details")]
        [SwaggerOperation(
            Summary = nameof(GetScheduleFollowUpOrderByPatientId),
            Description = GetScheduleFollowUpOrderByPatientIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetScheduleFollowUpOrderByPatientIdDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public ActionResult<IEnumerable<PractitionerMockData>> GetScheduleFollowUpOrderByPatientId(
            [FromRoute, SwaggerParameter("patientId")] string patientId)
        {
            Logger.LogDebug("Request to get hard coded json data");

            List<FollowupDetailsMockData> followupDetailsMockData = new List<FollowupDetailsMockData>();
            followupDetailsMockData.Add(new FollowupDetailsMockData
            {
                Id = "2032",
                HumanName = new HumanName
                {
                    FamilyName = "Fitzgerald",
                    GivenNames = new string[] { "Michael", "E" },
                    Suffixes = new string[] { "PCP" }
                },
                Photo = new Photo
                {
                    ContentType = "image/jpg",
                    Url = "https://dmgwebprodstorage.blob.core.windows.net/dmgprodweb/physician-headshots/Fitzgerald_Michael_FM-003websize.jpg"
                },
                Specialty = "Physician",
                Location = new Contracts.MockData.Location
                {
                    Id = "dsJKHasd.87Hts",
                    Address = new Contracts.MockData.Address
                    {
                        AddressLine = "1121 South Blvd",
                        AddressLine2 = "Suite 100",
                        City = "Oak Park",
                        State = "Illinois",
                        ZipeCode = "60302"
                    },
                    GeographicCoordinates = new GeographicCoordinates
                    {
                        Latitude = "41.86151314141414",
                        Longitude = "-87.93659437373738"
                    },
                    PhoneNumber = "(630) 893-2210",
                    Distance = 10
                }
            });

            return Ok(followupDetailsMockData);
        }

        /// <summary>
        /// Returns hard coded data.
        /// </summary>
        /// <param name="patientId">Id of LabandImagingLocations.</param>
        /// <returns>Returns hard coded json data.</returns>
        [HttpGet("/v1/appointment/{patientId}/location-details")]
        [SwaggerOperation(
            Summary = nameof(GetLabandImagingLocations),
            Description = GetLabandImagingLocationsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetLabandImagingLocationsDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public ActionResult<IEnumerable<PractitionerMockData>> GetLabandImagingLocations(
            [FromRoute, SwaggerParameter("patientId")] string patientId)
        {
            Logger.LogDebug("Request to get hard coded json data");

            List<LocationDetailsMockData> locationDetailsMockData = new List<LocationDetailsMockData>();
            locationDetailsMockData.Add(new LocationDetailsMockData
            {
                Name = "Oak Park",
                Location = new Contracts.MockData.Location
                {
                    Address = new Contracts.MockData.Address
                    {
                        AddressLine = "1133 South Blvd",
                        City = "Oak Park",
                        State = "IL",
                        ZipeCode = "60302"
                    },
                    PhoneNumber = "(630) 928-0386",
                    WorkingHours = "9 AM - 7 PM",
                    Distance = 23.01
                }
            });
            locationDetailsMockData.Add(new LocationDetailsMockData
            {
                Name = "Naperville",
                Location = new Contracts.MockData.Location
                {
                    Address = new Contracts.MockData.Address
                    {
                        AddressLine = "400 S. Eagle Street",
                        City = "Naperville",
                        State = "IL",
                        ZipeCode = "60563"
                    },
                    PhoneNumber = "(432) 740-7232",
                    WorkingHours = "8 AM - 6 PM",
                    Distance = 0
                }
            });
            locationDetailsMockData.Add(new LocationDetailsMockData
            {
                Name = "Wheaton",
                Location = new Contracts.MockData.Location
                {
                    Address = new Contracts.MockData.Address
                    {
                        AddressLine = "303 W Wesley Street",
                        City = "Wheaton",
                        State = "IL",
                        ZipeCode = "60187"
                    },
                    PhoneNumber = "(484) 465-7745",
                    WorkingHours = "8 AM - 7 PM",
                    Distance = 8.64
                }
            });
            locationDetailsMockData.Add(new LocationDetailsMockData
            {
                Name = "St. Charles",
                Location = new Contracts.MockData.Location
                {
                    Address = new Contracts.MockData.Address
                    {
                        AddressLine = "1125 North Arck",
                        City = "St. Charles",
                        State = "IL",
                        ZipeCode = "60119"
                    },
                    PhoneNumber = "(043) 987-1074",
                    WorkingHours = "9 AM - 6 PM",
                    Distance = 38.48
                }
            });
            locationDetailsMockData.Add(new LocationDetailsMockData
            {
                Name = "Cook Count",
                Location = new Contracts.MockData.Location
                {
                    Address = new Contracts.MockData.Address
                    {
                        AddressLine = "366 Downers Grove",
                        City = "Cook Count",
                        State = "IL",
                        ZipeCode = "60004"
                    },
                    PhoneNumber = "(630) 928-0386",
                    WorkingHours = "9 AM - 7 PM",
                    Distance = 32.19
                }
            });
            locationDetailsMockData.Add(new LocationDetailsMockData
            {
                Name = "Lombard",
                Location = new Contracts.MockData.Location
                {
                    Address = new Contracts.MockData.Address
                    {
                        AddressLine = "1987 Oak Brook",
                        City = "Lombard",
                        State = "IL",
                        ZipeCode = "60137"
                    },
                    PhoneNumber = "(484) 465-7745",
                    WorkingHours = "8 AM - 7 PM",
                    Distance = 10.87
                }
            });

            return Ok(locationDetailsMockData);
        }

        /// <summary>
        /// Returns Returns hard coded data.
        /// </summary>
        /// <param name="patientId">Id of ReferralDetails.</param>
        /// <returns>Returns hard coded data..</returns>
        [HttpGet("/v1/appointment/{patientId}/referral-details")]
        [SwaggerOperation(
            Summary = nameof(GetReferralDetailsByPatientId),
            Description = GetReferralDetailsByPatientIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetReferralDetailsByPatientIdDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public ActionResult<IEnumerable<AppointmentMockData>> GetReferralDetailsByPatientId(
            [Required, FromRoute, SwaggerParameter("AppointmentId")] string patientId)
        {
            Logger.LogDebug("Request to get hard coded json data");
            List<AppointmentMockData> appointmentMockData = new List<AppointmentMockData>();
            appointmentMockData.Add(new AppointmentMockData { ProviderType = "Gastroenterology" });
            appointmentMockData.Add(new AppointmentMockData { ProviderType = "Rehabilitation" });
            appointmentMockData.Add(new AppointmentMockData { ProviderType = "Endocrinology" });

            return Ok(appointmentMockData);
        }
    }
}