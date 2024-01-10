// <copyright file="SendSMSController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
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
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Controllers
{
    [Route("sendsms/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class SendSmsController : DulyControllerBase
    {
        private const string DescriptionSendSMSController = "Send SMS";

        private readonly ISendSmsAfterVisitedService _service;
        private readonly IIngestionClient _ingestionClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendSmsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="service">An instance of Post follow Up service.</param>
        /// <param name="ingestionClient">An instance of Ingestion service.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public SendSmsController(
            ISendSmsAfterVisitedService service,
            IIngestionClient ingestionClient,
            ILogger<SendSmsController> logger,
            IWebHostEnvironment environment)
            : base(logger, environment)
        {
            _service = service;
            _ingestionClient = ingestionClient;
        }

        [HttpPost]
        [SwaggerOperation(
           Summary = nameof(SendAfterVisitPdfSms),
           Description = DescriptionSendSMSController)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionSendSMSController, typeof(SendAfterVisitPdfSmsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [SwaggerOperationFilter(typeof(RequestBodyDescriptionFilter))]
        public async Task<ActionResult<SendAfterVisitPdfSmsResponse>> SendAfterVisitPdfSms([FromBody] SendSms model)
        {
            var response = await _service.SendAfterVisitPdfSms(model);
            var smsResponse = SendRequest(response);
            return StatusCode(StatusCodes.Status200OK, smsResponse);
        }

        private SendAfterVisitPdfSmsResponse SendRequest(Contracts.SendAfterVisitPdfSms message)
        {
            var sendSMSResponse = new SendAfterVisitPdfSmsResponse();
            var response = _ingestionClient.SendRequest(message);
            if (response.IsSuccessStatusCode)
            {
                sendSMSResponse.StatusCode = StatusCodes.Status200OK.ToString();
                sendSMSResponse.Message = "SMS sent successfully";
            }
            else
            {
                sendSMSResponse.StatusCode = StatusCodes.Status400BadRequest.ToString();
                sendSMSResponse.Message = "SMS not sent successfully";
            }

            return sendSMSResponse;
        }
    }
}