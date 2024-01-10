// <copyright file="ProvidersController.cs" company="Duly Health and Care">
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
    public class ProvidersController : DulyControllerBase
    {
        private const string GetRecommendedProvidersByReferralIdDescription = "Returns an array of referral providers for specific referral";
        private readonly IRecommendedProviderRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvidersController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        /// <param name="repository">A repository for working with <see cref="RecommendedProvider"/>.</param>
        public ProvidersController(
            ILogger<ProvidersController> logger,
            IWebHostEnvironment environment,
            IRecommendedProviderRepository repository)
            : base(logger, environment)
        {
            _repository = repository;
        }

        [HttpGet("referrals/{referralId}/[controller]")]
        [SwaggerOperation(
            Summary = nameof(GetRecommendedProvidersByReferralId),
            Description = GetRecommendedProvidersByReferralIdDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetRecommendedProvidersByReferralIdDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<RecommendedProvider>>> GetRecommendedProvidersByReferralId(
            [Required, FromRoute, SwaggerParameter("Referral Id")] string referralId)
        {
            var data = await _repository.GetRecommendedProvidersByReferralIdAsync(referralId);

            Validate(data);

            return Ok(data);
        }
    }
}
