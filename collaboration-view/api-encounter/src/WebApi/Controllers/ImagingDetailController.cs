// <copyright file="ImagingDetailController.cs" company="Duly Health and Care">
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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Device.Location;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Controllers
{
    [Route("imagingdetail/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class ImagingDetailController : DulyControllerBase
    {
        private const string DescriptionSaveImagingDetail = "Save ImagingDetail data";
        private const string DescriptionScheduleImaging = "Schedule Imaging data";

        private readonly IImagingDetailService _imagingDetailsService;
        private readonly IScheduleSlotsservice _scheduleservice;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagingDetailController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="imagingDetailsService">An instance of Patient service.</param>
        /// <param name="scheduleservice">An instance of Schedule Slot service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public ImagingDetailController(
               IImagingDetailService imagingDetailsService,
               ILogger<ImagingDetailController> logger,
               IScheduleSlotsservice scheduleservice,
               IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _imagingDetailsService = imagingDetailsService;
            _scheduleservice = scheduleservice;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = nameof(PostImagingDetail),
            Description = DescriptionSaveImagingDetail)]
        [SwaggerResponse(StatusCodes.Status201Created, DescriptionSaveImagingDetail, typeof(CreationResultResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [SwaggerOperationFilter(typeof(RequestBodyDescriptionFilter))]
        public async Task<ActionResult<CreationResultResponse>> PostImagingDetail([FromBody] ImagingDetail request)
        {
            var result = new CreationResultResponse();
            try
            {
                var record_Id = await _imagingDetailsService.PostImagingDetailAsync(request);
                result.RecordID = record_Id;
                result.CreationDate = System.DateTime.Now;
                result.ErrorMessage = string.Empty;
                result.StatusCode = StatusCodes.Status201Created.ToString();

                return StatusCode(StatusCodes.Status201Created, result);
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

        [HttpPost("ScheduleImaging")]
        [SwaggerOperation(
            Summary = nameof(ScheduleImagingAppointment),
            Description = DescriptionScheduleImaging)]
        [SwaggerResponse(StatusCodes.Status201Created, DescriptionSaveImagingDetail, typeof(CreationResultResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [SwaggerOperationFilter(typeof(RequestBodyDescriptionFilter))]
        public async Task<ActionResult<ScheduleAppointmentResult>> ScheduleImagingAppointment([Required, FromBody] ImagingSchedulingModel imagingSchedulingRequest)
        {
            imagingSchedulingRequest.ProviderId.Remove(imagingSchedulingRequest.SelectedProviderId);
            imagingSchedulingRequest.ProviderId.Insert(0, imagingSchedulingRequest.SelectedProviderId);
            ScheduleAppointmentResult data;
            foreach (var item in imagingSchedulingRequest.ProviderId)
            {
                try
                {
                    ScheduleAppointmentModel appointmentSchedulingRequest = new()
                    {
                        ProviderId = "External|" + item,
                        Date = imagingSchedulingRequest.Date,
                        DepartmentId = "External|" + imagingSchedulingRequest.DepartmentId,
                        Time = imagingSchedulingRequest.Time,
                        VisitTypeId = "External|4576",
                        PatientId = imagingSchedulingRequest.PatientId
                    };

                    data = await _scheduleservice.ScheduleImagingAppointmentForPatientAsync(appointmentSchedulingRequest);
                    Validate(data);

                    return data;
                }
                catch
                {
                    continue;
                }
            }

            return StatusCode(StatusCodes.Status400BadRequest, "No Slots Available");
        }

        [HttpGet("ImagingLocation")]
        [SwaggerOperation(
             Summary = nameof(GetImagingLocations),
             Description = DescriptionSaveImagingDetail)]
        [SwaggerResponse(StatusCodes.Status201Created, DescriptionSaveImagingDetail, typeof(CreationResultResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [SwaggerOperationFilter(typeof(RequestBodyDescriptionFilter))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public ActionResult<List<ImagingLocation>> GetImagingLocations(string service_slug, double lat, double lng)
        {
            List<ImagingLocation> imglocation = new List<ImagingLocation>();

            List<string> _provid = new List<string>();
            _provid.Add("3940");
            _provid.Add("6003");
            List<DeptProvider> dtarray = new List<DeptProvider>
            {
                new DeptProvider { Dept_id = "20876", Provider_ids = _provid }
            };
            var objAddressPart = new ImagingAddressPart
            {
                Street_number = "40",
                Number = "40",
                Address = "S. Clay St.",
                City = "Hinsdale",
                Postcode = "60521",
                County = "DuPage County",
                State = "Illinois",
                Country = "United States",
                Planet = "Earth",
                System = "the Solar System",
                Arm = "Orion Arm",
                Galaxy = "Milky Way",
                Group = "the Local Group",
                Cluster = "Virgo Cluster",
                Supercluster = "Laniakea Supercluster"
            };
            var objAddress = new ImagingAddress
            {
                Address = "40 S. Clay St., Hinsdale, IL 60521",
                Lng = -87.93608844280244,
                Lat = 41.802066721137315,
                Parts = objAddressPart
            };
            var result = new ImagingLocation
            {
                Id = 19735,
                Title = "40 S. Clay St., Hinsdale, IL 60521",
                DateCreated = new System.DateTime(2020, 06, 18),
                DateUpdated = new System.DateTime(2020, 06, 03),
                Address = objAddress,
                ProviderIds = dtarray
            };

            result.Distance = GetDistance(lat, lng, result.Address.Lat, result.Address.Lng);

            // second Object
            List<string> _provid1 = new List<string>();
            _provid1.Add("3812");
            List<DeptProvider> dtarray1 = new List<DeptProvider>
            {
                new DeptProvider { Dept_id = "20376", Provider_ids = _provid1 }
            };

            var objAddressPart1 = new ImagingAddressPart
            {
                Street_number = "430",
                Number = "430",
                Address = "Warrenville Road",
                City = "Lisle",
                Postcode = "60532",
                County = "DuPage County",
                State = "Illinois",
                Country = "United States",
                Planet = "Earth",
                System = "the Solar System",
                Arm = "Orion Arm",
                Galaxy = "Milky Way",
                Group = "the Local Group",
                Cluster = "Virgo Cluster",
                Supercluster = "Laniakea Supercluster"
            };

            var objAddress1 = new ImagingAddress
            {
                Address = "430 Warrenville Rd, Lisle, IL 60532",
                Lng = -88.0591106414795,
                Lat = 41.80912441900324,
                Parts = objAddressPart1
            };

            var result1 = new ImagingLocation
            {
                Id = 38224,
                Title = "430 Warrenville Rd, Lisle, IL 60532",
                DateCreated = new System.DateTime(2020, 06, 18),
                DateUpdated = new System.DateTime(2020, 06, 03),
                Address = objAddress1,
                ProviderIds = dtarray1
            };

            result1.Distance = GetDistance(lat, lng, result1.Address.Lat, result1.Address.Lng);

            //Third object
            List<string> _provid2 = new List<string>();
            _provid2.Add("6003");
            List<DeptProvider> dtarray2 = new List<DeptProvider>
            {
                new DeptProvider { Dept_id = "17054", Provider_ids = _provid2 }
            };

            var objAddressPart2 = new ImagingAddressPart
            {
                Street_number = "1801",
                Number = "1801",
                Address = "S. Highland Ave.",
                City = "Lombard",
                Postcode = "60148",
                County = "DuPage County",
                State = "Illinois",
                Country = "United States",
                Planet = "Earth",
                System = "the Solar System",
                Arm = "Orion Arm",
                Galaxy = "Milky Way",
                Group = "the Local Group",
                Cluster = "Virgo Cluster",
                Supercluster = "Laniakea Supercluster"
            };

            var objAddress2 = new ImagingAddress
            {
                Address = "1801, S. Highland Ave., Lombard, 60148, DuPage County, Illinois, United States",
                Lng = -88.01121711730957,
                Lat = 41.849536250383814,
                Parts = objAddressPart2
            };

            var result2 = new ImagingLocation
            {
                Id = 34222,
                Title = "1801, S. Highland Ave., Lombard, 60148, DuPage County, Illinois, United States",
                DateCreated = new System.DateTime(2020, 06, 18),
                DateUpdated = new System.DateTime(2020, 06, 03),
                Address = objAddress2,
                ProviderIds = dtarray2
            };

            result2.Distance = GetDistance(lat, lng, result2.Address.Lat, result2.Address.Lng);

            imglocation.Add(result);
            imglocation.Add(result1);
            imglocation.Add(result2);

            return StatusCode(StatusCodes.Status200OK, imglocation);
        }

        private double GetDistance(double sLatitude, double sLongitude, double eLatitude, double eLongitude)
        {
            var sCoord = new GeoCoordinate(sLatitude, sLongitude);
            var eCoord = new GeoCoordinate(eLatitude, eLongitude);

            return sCoord.GetDistanceTo(eCoord);
        }
    }
}
