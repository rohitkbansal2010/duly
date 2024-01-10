// <copyright file="ServiceRequestController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Common.Annotations.Constants;
using Duly.Common.ApiExtensions.Constants;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
using Duly.Common.Infrastructure.Exceptions;
using Duly.Common.Security.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
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
    public class ServiceRequestController : DulyControllerBase
    {
        private const string DescriptionGetLabAndImagingOrders = "Returns Lab and Imaging orders";
        private readonly IServiceRequestRepository _serviceRequestRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRequestController" /> class.
        /// </summary>
        /// <param name="serviceRequestRepository">A repository for working on <see cref="ServiceRequest"/>.</param>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public ServiceRequestController(
            IServiceRequestRepository serviceRequestRepository,
            ILogger<ServiceRequestController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _serviceRequestRepository = serviceRequestRepository;
        }

        /// <summary>
        /// Returns a <see cref="ServiceRequest"/> with details.
        /// </summary>
        /// <param name="patientId">Id of a specific patient.</param>
        /// <param name="appointmentId">encounter appointmentId.</param>
        /// <param name="type">Labs or Imaging.</param>
        /// <returns>Returns lab and Imaging orders for the patient based on status.</returns>
        [HttpGet(RoutePaths.PatientIdName + "/appointmentId/{appointmentId}/type/{type}")]
        [SwaggerOperation(Summary = nameof(GetLabOrders), Description = DescriptionGetLabAndImagingOrders)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionGetLabAndImagingOrders)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [ObfuscatedIdentityParameters(new[] { "patientId" })]
        public async Task<ActionResult<ServiceRequest>> GetLabOrders(
            [Required, FromRoute, SwaggerParameter(RoutePaths.PatientIdDescription)] string patientId, [Required, FromRoute] string appointmentId ,[Required, FromRoute] string type)
        {
            var data = await _serviceRequestRepository.GetLabOrdersAsync(patientId, appointmentId, type);

            if (data == null)
            {
                throw new EntityNotFoundException(nameof(Patient), patientId);
            }

            Validate(data);

            return Ok(data);
        }
    }
}
