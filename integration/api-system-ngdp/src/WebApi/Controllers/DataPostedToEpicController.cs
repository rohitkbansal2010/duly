// <copyright file="DataPostedToEpicController.cs" company="Duly Health and Care">
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
    public class DataPostedToEpicController : DulyControllerBase
    {
        private const string DataPostedToEpicDescription = "Updates the database if Data is posted to epic";


        private readonly IDataPostedToEpicRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPostedToEpicController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        /// <param name="repository">A repository for working with <see cref=""/>.</param>
        public DataPostedToEpicController(
            IDataPostedToEpicRepository repository,
            ILogger<DataPostedToEpicController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _repository = repository;
        }

        [HttpPost("[controller]/id/{id}")]
        [SwaggerOperation(
             Summary = nameof(DataPostedToEpic),
             Description = DataPostedToEpicDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, DataPostedToEpicDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<int>> DataPostedToEpic(
            [Required, FromRoute] int id)
        {
            var data = await _repository.DataPostedToEpicAsync(id);

            return Ok(data);
        }
    }
}
