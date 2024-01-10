// <copyright file="AfterVisitPdfController.cs" company="Duly Health and Care">
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
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class AfterVisitPdfController : DulyControllerBase
    {
        private const string CreateAfterVisitDescription = "Save the After visit data.";
        private const string GetAfterVisitDescription = "Get the After visit data.";

        private readonly IAfterVisitPdfRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AfterVisitPdfController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        /// <param name="repository">A repository for working with <see cref="after"/>.</param>
        public AfterVisitPdfController(
           ILogger<AfterVisitPdfController> logger,
           IWebHostEnvironment environment,
           IAfterVisitPdfRepository repository)
           : base(logger, environment)
        {
            _repository = repository;
        }

        [HttpPost("[controller]")]
        [SwaggerOperation(
             Summary = nameof(PostAfterVisitPdf),
             Description = CreateAfterVisitDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, CreateAfterVisitDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<int>> PostAfterVisitPdf([FromBody] AfterVisitPdf request)
        {
            var data = await _repository.PostAfterVisitPdfAsync(request);

            return Ok(data);
        }

        [HttpGet("[controller]")]
        [SwaggerOperation(
             Summary = nameof(GetAfterVisitPdfById),
             Description = GetAfterVisitDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetAfterVisitDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<string>> GetAfterVisitPdfById(long afterVisitPdfId)
        {
            var data = await _repository.GetAfterVisitPdfByAfterVisitPdfIdAsync(afterVisitPdfId);

            return Ok(data);
        }
    }
}
