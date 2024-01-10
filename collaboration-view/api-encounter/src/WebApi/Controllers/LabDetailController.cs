// <copyright file="LabDetailController.cs" company="Duly Health and Care">
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
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Controllers
{
    [Route("labdetail/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class LabDetailController : DulyControllerBase
    {
        private const string GetLabLocationByLatLngDescription = "Returns the Details of the lab on the basis of latitude and longitude.";

        private readonly ILabDetailService _labDetailservice;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabDetailController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="labDetailService">An instance of Patient service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public LabDetailController(
             ILabDetailService labDetailService,
             ILogger<LabDetailController> logger,
             IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _labDetailservice = labDetailService;
        }

        [HttpPost]
        [SwaggerOperation(nameof(PostLabDetail), "Creates a new Lab Detail.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Returns LabDetail .", typeof(CreationResultResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Returns a validation error result.", typeof(FaultResponse))]
        [SwaggerOperationFilter(typeof(RequestBodyDescriptionFilter))]
        public async Task<ActionResult<CreationResultResponse>> PostLabDetail([FromBody] LabDetail request)
        {
            var result = new CreationResultResponse();
            var record_Id = await _labDetailservice.PostLabDetailAsync(request);
            result.RecordID = record_Id;
            result.CreationDate = System.DateTime.Now;
            result.StatusCode = StatusCodes.Status201Created.ToString();
            result.ErrorMessage = string.Empty;

            return StatusCode(StatusCodes.Status201Created, result);
        }

        /// <summary>
        /// Returns <see cref="LabLocation"/> for a specific patient.
        /// </summary>
        /// <param name="lat">Latitude of a specific Location.</param>
        /// <param name="lng">Longitude of a specific Location.</param>
        /// <returns>Retrieved <see cref="LabLocation"/> instance.</returns>
        [HttpGet("ByLatLng/{lat}/{lng}")]
        [SwaggerOperation(
        Summary = nameof(GetLabLocationByLatLng),
        Description = GetLabLocationByLatLngDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns Lab Location by lat lng.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<LabLocation>> GetLabLocationByLatLng(string lat, string lng)
        {
            var data = await _labDetailservice.GetLabLocationByLatLngAsync(lat, lng);
            Validate(data);

            return Ok(data);
        }
    }
}
