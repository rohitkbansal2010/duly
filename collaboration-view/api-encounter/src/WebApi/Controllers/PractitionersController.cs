// -----------------------------------------------------------------------
// <copyright file="PractitionersController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Annotations.Constants;
using Duly.Common.ApiExtensions.Constants;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class PractitionersController : DulyControllerBase
    {
        private const string GetPractitionerGeneralInfosOfSpecificSiteDescription = "Returns general information about each available practitioner for a specific site.";
        private const string GetPractitionerSlotsDescription = "Returns the collection of Practitioner By Date.";

        private readonly IPractitionerService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="PractitionersController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="service">An instance of Practitioner service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public PractitionersController(
            IPractitionerService service,
            ILogger<PractitionersController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _service = service;
        }

        /// <summary>
        /// Returns <see cref="PractitionerGeneralInfo"/> about each available practitioner for a specific site.
        /// </summary>
        /// <param name="siteId">Id of a specific site.</param>
        /// <returns>An array of PractitionerGeneralInfo of each available practitioner for a specific site.</returns>
        [HttpGet("/" + RoutePaths.DefaultSiteRoute)]
        [SwaggerOperation(
            Summary = nameof(GetPractitionersBySiteId),
            Description = GetPractitionerGeneralInfosOfSpecificSiteDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetPractitionerGeneralInfosOfSpecificSiteDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<PractitionerGeneralInfo>>> GetPractitionersBySiteId(
            [Required, FromRoute, SwaggerParameter("Site Id")] string siteId)
        {
            Logger.LogDebug("Request general information about each available practitioner for a specific site '{SiteId}'", siteId);

            var practitioners = await _service.GetPractitionersBySiteIdAsync(siteId);

            Validate(practitioners);

            return Ok(practitioners);
        }

        /// <summary>
        /// Returns hard coded data.
        /// </summary>
        /// /// <param name="practitionerId">Id of a specific site.</param>
        /// <returns>Returns hard coded json data.</returns>
        [HttpGet("/v1/practitioner/{practitionerId}/slots")]
        [SwaggerOperation(
            Summary = nameof(GetPractitionerSlotsByDate),
            Description = GetPractitionerSlotsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetPractitionerSlotsDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public ActionResult<IEnumerable<PractitionerMockData>> GetPractitionerSlotsByDate(
            [FromRoute, SwaggerParameter("PractitionerId")] int practitionerId)
        {
            Logger.LogDebug("Request to get hard coded json data.");

            List<PractitionerMockData> practitionerMockData = new List<PractitionerMockData>();
            practitionerMockData.Add(new PractitionerMockData { Time = "30", DisplayTime = "09:30 AM" });
            practitionerMockData.Add(new PractitionerMockData { Time = "30", DisplayTime = "09:45 AM" });
            practitionerMockData.Add(new PractitionerMockData { Time = "30", DisplayTime = "10:30 AM" });
            practitionerMockData.Add(new PractitionerMockData { Time = "30", DisplayTime = "12:45 PM" });
            practitionerMockData.Add(new PractitionerMockData { Time = "30", DisplayTime = "01:00 PM" });
            practitionerMockData.Add(new PractitionerMockData { Time = "30", DisplayTime = "01:30 PM" });
            practitionerMockData.Add(new PractitionerMockData { Time = "30", DisplayTime = "01:45 PM" });
            practitionerMockData.Add(new PractitionerMockData { Time = "30", DisplayTime = "02:30 PM" });
            practitionerMockData.Add(new PractitionerMockData { Time = "30", DisplayTime = "03:30 PM" });
            practitionerMockData.Add(new PractitionerMockData { Time = "30", DisplayTime = "03:45 PM" });
            practitionerMockData.Add(new PractitionerMockData { Time = "30", DisplayTime = "04:30 PM" });
            practitionerMockData.Add(new PractitionerMockData { Time = "30", DisplayTime = "05:00 PM" });
            practitionerMockData.Add(new PractitionerMockData { Time = "30", DisplayTime = "06:00 PM" });
            practitionerMockData.Add(new PractitionerMockData { Time = "30", DisplayTime = "06:30 PM" });
            practitionerMockData.Add(new PractitionerMockData { Time = "30", DisplayTime = "06:45 PM" });
            practitionerMockData.Add(new PractitionerMockData { Time = "30", DisplayTime = "07:00 PM" });
            practitionerMockData.Add(new PractitionerMockData { Time = "30", DisplayTime = "07:15 PM" });
            practitionerMockData.Add(new PractitionerMockData { Time = "30", DisplayTime = "07:45 PM" });
            practitionerMockData.Add(new PractitionerMockData { Time = "30", DisplayTime = "08:0 PM" });

            if (practitionerId % 2 == 0)
            {
                return Ok(practitionerMockData);
            }
            else
            {
                List<PractitionerMockData> practitionerMock = new List<PractitionerMockData>();
                return practitionerMock;
            }
        }
    }
}