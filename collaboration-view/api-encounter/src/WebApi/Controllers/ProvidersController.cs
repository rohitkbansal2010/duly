// <copyright file="ProvidersController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Annotations.Constants;
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
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class ProvidersController : DulyControllerBase
    {
        private const string GetProvidersByLatLngDescription = "Returns closest providers by lat lng";
        private const string GetProviderDetailsDescription = "Returns the Details of the Provider on the basis of Provider Id.";

        private readonly IProviderService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvidersController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="service">An instance of Practitioner service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public ProvidersController(
            IProviderService service,
            ILogger<ProvidersController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _service = service;

            /// <summary>
            /// Returns <see cref="Provider"/> for a specific patient.
            /// </summary>
            /// <param name="lat">Latitude of a specific Location.</param>
            /// <param name="lng">Longitude of a specific Location.</param>
            /// <param name="providerType">providerType.</param>
            /// <returns>Retrieved <see cref="Provider"/> instance.</returns>
        }

        [HttpGet("ByLatLng/{lat}/{lng}/{providerType}")]
        [SwaggerOperation(
          Summary = nameof(GetProvidersByLatLng),
          Description = GetProvidersByLatLngDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns Providesr by lat lng.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<Provider>> GetProvidersByLatLng(string lat, string lng, string providerType)
        {
            var data = await _service.GetProvidersByLatLngAsync(lat, lng, providerType);
            Validate(data);

            return Ok(data);
        }

        /// <summary>
        /// Returns <see cref="ProviderDetails"/> that represents an information of the provider.
        /// </summary>
        /// <param name="providerIds">Id of a specific provider.</param>
        /// <returns>An instance of <see cref="ProviderDetails"/> for a specific provider.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = nameof(GetProviderDetails),
            Description = GetProviderDetailsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetProviderDetailsDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<ProviderDetails>> GetProviderDetails([Required, FromQuery] string providerIds)
        {
            var data = await _service.GetProviderDetailsAsync(providerIds);

            Validate(data);

            return Ok(data);
        }
    }
}
