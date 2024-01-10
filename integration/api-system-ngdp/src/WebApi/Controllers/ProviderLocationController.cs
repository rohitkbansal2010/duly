// <copyright file="ProviderLocationController.cs" company="Duly Health and Care">
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
    public class ProviderLocationController : DulyControllerBase
    {
        private const string GetProvidersByLatLngDescription = "Returns an array of closest providers ";
        private const string GetProviderDetailsDescription = "Returns the Details of the Provider on the basis of Provider Id.";

        private readonly IProviderRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderLocationController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        /// <param name="repository">A repository for working with <see cref=""/>.</param>
        public ProviderLocationController(
           ILogger<ProviderLocationController> logger,
           IWebHostEnvironment environment,
           IProviderRepository repository)
           : base(logger, environment)
        {
            _repository = repository;
        }

        /// <summary>
        /// Returns all available <see cref="ProviderLocation"/> for a specific location.
        /// </summary>
        /// <param name="lat">Location Id.</param>
        /// <param name="lng">Filter by start date.</param>
        /// <param name="providerType">Filter by end date.</param>
        /// <returns>An array of ProvidersByLatLng.</returns>
        [HttpGet("latlng/{lat}/{lng}/{providerType}/[controller]")]
        [SwaggerOperation(
         Summary = nameof(GetProvidersByLatLng),
         Description = GetProvidersByLatLngDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetProvidersByLatLngDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<ProviderLocation>>> GetProvidersByLatLng(string lat, string lng, string providerType)
        {
            var data = await _repository.GetProvidersByLatLng(lat, lng, providerType);

            Validate(data);

            return Ok(data);
        }

        /// <summary>
        /// Returns all available <see cref="ProviderDetails"/> for a specific provider.
        /// </summary>
        /// <param name="providerIds">Provider Id.</param>
        /// <returns>Provider details on the Basis of provider id.</returns>
        [HttpGet("Providers")]
        [SwaggerOperation(
            Summary = nameof(GetProviderDetails),
            Description = GetProviderDetailsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetProviderDetailsDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<ProviderDetails>>> GetProviderDetails([Required, FromQuery] string providerIds)
        {
            var data = await _repository.GetProviderDetailsAsync(providerIds);

            Validate(data);

            return Ok(data);
        }
    }
}