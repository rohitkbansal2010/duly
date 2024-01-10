// -----------------------------------------------------------------------
// <copyright file="AfterVisitPdfController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Annotations.Constants;
using Duly.Common.Annotations.Filters;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
using Duly.Common.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Controllers
{
    [Route("aftervisitpdf/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = Constants.ScopesConfigurationKey)]
    public class AfterVisitPdfController : DulyControllerBase
    {
        private const string DescriptionAfterVisitPdfDetail = "Save After data";
        private const string DescriptionGetAfterVisitPdfDetail = "Get After visit pdf data";

        private readonly SendSmsController _sendSMSController;
        private readonly IAfterVisitPdfService _afterVisitPdfService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AfterVisitPdfController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="afterVisitPdfService">An instance of Patient service.</param>
        /// <param name="sendSMSController">An instance of SendSMSController controller.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        public AfterVisitPdfController(
               IAfterVisitPdfService afterVisitPdfService,
               ILogger<AfterVisitPdfController> logger,
               IWebHostEnvironment environment,
               SendSmsController sendSMSController)
            : base(logger, environment)
        {
            _afterVisitPdfService = afterVisitPdfService;
            _sendSMSController = sendSMSController;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = nameof(PostAfterVisitPdf),
            Description = DescriptionAfterVisitPdfDetail)]
        [SwaggerResponse(StatusCodes.Status201Created, DescriptionAfterVisitPdfDetail, typeof(CreationResultResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        [SwaggerOperationFilter(typeof(RequestBodyDescriptionFilter))]
        public async Task<ActionResult<CreationResultResponse>> PostAfterVisitPdf([FromBody] AfterVisitPdf request)
        {
            try
            {
                var result = new PostAfterVisitPdfResponse();
                var afterVisitPdfId = await _afterVisitPdfService.PostAfterVisitPdfAsync(request);
                result.AfterVisitPdfId = afterVisitPdfId;
                result.Message = string.Empty;
                if (request.TriggerSMS)
                {
                    SendSms sendSMSParam = new SendSms();
                    sendSMSParam.AppointmentId = Convert.ToString(request.AppointmentId);
                    sendSMSParam.PatientId = request.PatientId;
                    sendSMSParam.PdfId = Convert.ToString(afterVisitPdfId);
                    sendSMSParam.PhoneNumber = request.PhoneNumber;

                    await _sendSMSController.SendAfterVisitPdfSms(sendSMSParam);

                    await _afterVisitPdfService.UpdateAfterVisitPdfIsSMSSentAsync(afterVisitPdfId, true);
                }

                result.StatusCode = StatusCodes.Status201Created.ToString();
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (SqlException ex)
            {
                Logger.LogWarning(ex, ex.Message);
                return BadRequest(new FaultResponse { ErrorMessage = ex.Message });
            }
            catch (Exception e)
            {
                Logger.LogWarning(e, e.Message);
                return BadRequest(new FaultResponse { ErrorMessage = e.Message });
            }
        }

        [HttpGet("/[controller]/{aftervisitpdfid}")]
        [SwaggerOperation(
           Summary = nameof(GetAfterVisitPdfById),
           Description = DescriptionGetAfterVisitPdfDetail)]
        [SwaggerResponse(StatusCodes.Status200OK, DescriptionGetAfterVisitPdfDetail, typeof(GetAfterVisitPdfResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<GetAfterVisitPdfResponse>> GetAfterVisitPdfById([FromRoute] long aftervisitpdfid)
        {
            try
            {
                var afterVisitPdf = await _afterVisitPdfService.GetAfterVisitPdfById(aftervisitpdfid);
                var result = new GetAfterVisitPdfResponse();
                result.AfterVisitPdf = afterVisitPdf;
                result.StatusCode = StatusCodes.Status200OK;
                return Ok(result);
            }
            catch (SqlException ex)
            {
                Logger.LogWarning(ex, ex.Message);
                return BadRequest(new FaultResponse { ErrorMessage = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                Logger.LogWarning(ex, ex.Message);
                return NotFound(new FaultResponse { ErrorMessage = ex.Message });
            }
            catch (Exception e)
            {
                Logger.LogWarning(e, e.Message);
                return BadRequest(new FaultResponse { ErrorMessage = e.Message });
            }
        }
    }
}