// <copyright file="LabDetailsController.cs" company="Duly Health and Care">
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
    public class LabDetailsController : DulyControllerBase
    {
        private const string CreateLabDetailsDescription = "Save lab details of the patient.";
        private const string GetLabLocationByLatLngDescription = "Returns the Details of the lab on the basis of latitude and longitude.";
        private readonly ILabDetailsRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabDetailsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        /// <param name="repository">A repository for working with <see cref=""/>.</param>
        public LabDetailsController(
            ILabDetailsRepository repository,
            ILogger<LabDetailsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _repository = repository;
        }

        [HttpPost("[controller]")]
        [SwaggerOperation(
             Summary = nameof(PostLabDetails),
             Description = CreateLabDetailsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, CreateLabDetailsDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<int>> PostLabDetails([FromBody] LabDetails request)
        {
            var data = await _repository.PostLabDetailsAsync(request);

            return Ok(data);
        }

        /// <summary>
        /// Returns all available <see cref="LabLocation"/> for a specific location.
        /// </summary>
        /// <param name="lat">Location Id.</param>
        /// <param name="lng">Filter by start date.</param>
        /// /// <returns>An array of LabLocationByLatLng.</returns>
        [HttpGet("latlng/{lat}/{lng}/[controller]")]
        [SwaggerOperation(
         Summary = nameof(GetLabLocationByLatLng),
         Description = GetLabLocationByLatLngDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetLabLocationByLatLngDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<LabLocation>>> GetLabLocationByLatLng(string lat, string lng)
        {
            var data = await _repository.GetLabLocationByLatLng(lat, lng);

            Validate(data);

            return Ok(data);
        }
    }
}
