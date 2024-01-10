// <copyright file="GetCheckOutDetailsController.cs" company="Duly Health and Care">
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
    public class GetCheckOutDetailsController : DulyControllerBase
    {
        private const string GetCheckOutDetailsDescription = "Returns an array of Labdetails and schedulereferral / followup";

        private readonly ICheckOutDetailsRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCheckOutDetailsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        /// <param name="repository">A repository for working on <see cref="CheckOutDetails"/>.</param>
        public GetCheckOutDetailsController(
            ILogger<ImmunizationsController> logger,
            IWebHostEnvironment environment,
            ICheckOutDetailsRepository repository)
            : base(logger, environment)
        {
            _repository = repository;
        }

        /// <summary>
        /// Returns all available <see cref="CheckOutDetails"/> for a specific patient.
        /// </summary>
        /// <param name="appointmentId">Patient Id.</param>
        /// <returns>An array of immunizations for specific patient.</returns>
        [HttpGet("{appointmentId}/[controller]")]
        [SwaggerOperation(
            Summary = nameof(GetCheckOutDetails),
            Description = GetCheckOutDetailsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetCheckOutDetailsDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<CheckOutDetails>> GetCheckOutDetails(string appointmentId)
        {
            var data = await _repository.GetCheckOutDetailsAsync(appointmentId);

            Validate(data);

            return Ok(data);
        }
    }
}
