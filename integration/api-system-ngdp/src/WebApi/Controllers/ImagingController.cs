﻿// <copyright file="ImagingController.cs" company="Duly Health and Care">
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
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class ImagingController : DulyControllerBase
    {
        private const string PostImagingDescription = "Save imaging details of the patient.";

        private readonly IImagingRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagingController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        /// <param name="repository">A repository for working with <see cref=""/>.</param>
        public ImagingController(
            IImagingRepository repository,
            ILogger<ImagingController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _repository = repository;
        }

        [HttpPost("[controller]")]
        [SwaggerOperation(
             Summary = nameof(PostImaging),
             Description = PostImagingDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, PostImagingDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<int>> PostImaging([FromBody] ImagingDetail request)
        {
            var data = await _repository.PostImagingAsync(request);

            return Ok(data);
        }
    }
}
