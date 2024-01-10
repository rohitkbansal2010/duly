// <copyright file="Intake.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Azure.Messaging;
using Duly.Clinic.Audit.Ingestion.Exceptions;
using Duly.Clinic.Audit.Ingestion.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Duly.Clinic.Audit.Ingestion
{
    /// <summary>
    /// Defines the Azure functions to handle audit events from Event Grid Topic and Service Bus Queue.
    /// </summary>
    public class Intake
    {
        private const string WebhookFunctionName = "Integration-AuditIntakeWebhook";
        private const string QueueFunctionName = "Integration-AuditIntakeQueue";

        private readonly ICloudEventLogAdapter _cloudEventLogAdapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="Intake" /> class.
        /// </summary>
        /// <param name="cloudEventLogAdapter">An instance of <see cref="ICloudEventLogAdapter" /> class.</param>
        public Intake(ICloudEventLogAdapter cloudEventLogAdapter)
        {
            _cloudEventLogAdapter = cloudEventLogAdapter;
        }

        /// <summary>
        /// Implements the intake function to handle audit events via Webhook from Event Grid Topic.
        /// </summary>
        /// <param name="request">An original HTTP request data in <see cref="CloudEvent" /> format.</param>
        /// <param name="context">An instance of <see cref="FunctionContext" /> object.</param>
        /// <returns>
        /// An expected by EvenGrid HTTP response.
        /// </returns>
        /// <remarks>
        ///  The Azure Functions Event Grid binding does not natively support CloudEvents,
        ///  so HTTP-triggered functions are used to read CloudEvents messages (https://cloudevents.io/).
        ///  Source: https://docs.microsoft.com/en-us/azure/event-grid/cloudevents-schema#azure-functions.
        /// </remarks>
        [Function(WebhookFunctionName)]
        public async Task<HttpResponseData> GetCloudEventFromEventGridTopicAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", "options", Route = "intake")] HttpRequestData request,
            FunctionContext context)
        {
            var logger = context.GetLogger(WebhookFunctionName);

            if (request.Method == HttpMethod.Options.Method)
            {
                // Retrieve the request origin.
                if (!request.Headers.TryGetValues("WebHook-Request-Origin", out var headerValues))
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest);
                }

                var response = request.CreateResponse(HttpStatusCode.OK);

                // The CloudEvents implements its own abuse protection semantics using the HTTP OPTIONS method.
                // You can read more about it here - https://github.com/cloudevents/spec/blob/v1.0/http-webhook.md#4-abuse-protection.
                response.Headers.Add("WebHook-Allowed-Rate", "*");
                response.Headers.Add("WebHook-Allowed-Origin", headerValues.FirstOrDefault());

                return response;
            }

            try
            {
                // CloudEvents schema delivers one event at a time.
                var cloudEvent = CloudEvent.Parse(await BinaryData.FromStreamAsync(request.Body));

                return
                    await _cloudEventLogAdapter.SendCloudEventAsync(cloudEvent, logger)
                        ? request.CreateResponse(HttpStatusCode.OK)
                        : request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"The cloud event was not deserialized from the request body.");

                return request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Implements the intake function to handle audit events via Service Bus Queue.
        /// </summary>
        /// <param name="request">An original payload in <see cref="CloudEvent" /> format.</param>
        /// <param name="context">An instance of <see cref="FunctionContext" /> object.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        [Function(QueueFunctionName)]
        public async Task GetCloudEventFromServiceBusAsync(
            [ServiceBusTrigger("%SourceQueue%", Connection = "QueueConnectionString")] string request,
            FunctionContext context)
        {
            var logger = context.GetLogger(QueueFunctionName);

            var cloudEvent = CloudEvent.Parse(BinaryData.FromString(request));

            var result = await _cloudEventLogAdapter.SendCloudEventAsync(cloudEvent, logger);

            if (!result)
            {
                logger.LogError($"Failed to process cloud event [{cloudEvent.Id}] received from queue.");

                throw new LogAnalyticsEntryBuilderException("Failure during attempt to send an event into log analytics workspace.");
            }
        }
    }
}