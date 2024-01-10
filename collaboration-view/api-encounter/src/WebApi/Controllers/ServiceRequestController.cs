// -----------------------------------------------------------------------
// <copyright file="ServiceRequestController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Controllers
{
    /// <summary>
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-315.
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-339.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class ServiceRequestController : DulyControllerBase
    {
        private const string DescriptionGetLabOrImagingOrders = "Returns Lab or Imaging Orders";

        private readonly IServiceRequestService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRequestController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="service">An instance of Patient service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public ServiceRequestController(
            IServiceRequestService service,
            ILogger<ServiceRequestController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _service = service;
        }

        /// <summary>
        /// Returns <see cref="ServiceRequest"/> for a specific patient.
        /// </summary>
        /// <param name="patientId">Id of a specific patient.</param>
        /// <param name="appointmentId">appointment Id.</param>
        /// <param name="type">Labs or Imaging.</param>
        /// <returns>Retrieved <see cref="ServiceRequest"/> instance.</returns>
        [HttpGet(RoutePaths.PatientIdName + "/appointment/{appointmentId}/type/{type}")]
        [SwaggerOperation(
            Summary = nameof(GetLabOrImagingOrders),
            Description = DescriptionGetLabOrImagingOrders)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionGetLabOrImagingOrders)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<ServiceRequest>> GetLabOrImagingOrders(
            [Required, FromRoute, SwaggerParameter(RoutePaths.PatientIdDescription)] string patientId, [Required, FromRoute] string appointmentId, [Required, FromRoute] string type)
        {
            var Data = await _service.GetLabOrImagingOrdersAsync(patientId, appointmentId, type);

            Validate(Data);

            return Ok(Data);
        }
    }
}
