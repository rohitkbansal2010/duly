// -----------------------------------------------------------------------
// <copyright file="UsersController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Controllers
{
    /// <summary>
    /// https://jirapct.epam.com/jira/browse/DPGECLOF-2266.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class UsersController : DulyControllerBase
    {
        private const string GetActiveUserDescription = "Returns an active (logged-in) user";

        private readonly IUserService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="service">An instance of User service.</param>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public UsersController(
            IUserService service,
            ILogger<UsersController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _service = service;
        }

        /// <summary>
        /// Returns active (logged-in) <see cref="User"/>.
        /// </summary>
        /// <returns>An instance of <see cref="User"/>.</returns>
        [HttpGet("active")]
        [SwaggerOperation(
            Summary = nameof(GetActiveUser),
            Description = GetActiveUserDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns active user.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<User>> GetActiveUser()
        {
            var data = await _service.GetActiveUserAsync();

            Validate(data);

            return Ok(data);
        }
    }
}
