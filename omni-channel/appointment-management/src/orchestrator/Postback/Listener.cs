// <copyright file="Listener.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Azure.Messaging;
using Duly.OmniChannel.Orchestrator.Appointment.Common.Interfaces;
using Duly.OmniChannel.Orchestrator.Appointment.Common.Models;
using Duly.OmniChannel.Orchestrator.Appointment.Postback.Entities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Duly.OmniChannel.Orchestrator.Appointment.Postback
{
    /// <summary>
    /// Defines the Azure function to handle postback events received from Communication Hub.
    /// </summary>
    public class Listener
    {
        private const string FunctionName = "Appointment-Management-PostbackListener";
        private const string AppointmentScheduledCompletionCode = "APPOINTMENT.SCHEDULED";
        private const string ConfirmationSent = "CONFIRMATION.SENT";
        private const string ConfirmationFailed = "CONFIRMATION.FAILED";

        private readonly IIngestionClient _ingestionClient;
        private readonly IReferralRepository _referralRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Listener" /> class.
        /// </summary>
        /// <param name="ingestionClient">An instance of <see cref="IIngestionClient" /> class.</param>
        /// <param name="referralRepository">An instance of <see cref="IReferralRepository" /> class.</param>
        public Listener(
            IIngestionClient ingestionClient,
            IReferralRepository referralRepository)
        {
            _ingestionClient = ingestionClient;
            _referralRepository = referralRepository;
        }

        /// <summary>
        /// Represents an Azure Function to handle postback events received from Communication Hub.
        /// </summary>
        /// <param name="request">An original payload received from service providers.</param>
        /// <param name="context">An instance of <see cref="FunctionContext" /> object.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        [Function(FunctionName)]
        public async Task RunAsync(
            [ServiceBusTrigger("%SourceQueue%", Connection = "ServiceBusConnection")] string request,
            FunctionContext context)
        {
            var log = context.GetLogger<Listener>();

            try
            {
                var cloudEvent = CloudEvent.Parse(BinaryData.FromString(request));

                log.LogInformation("A notification event has been received (ID: {EventId}; Source: {EventSource}).", cloudEvent.Id, cloudEvent.Source);

                var data = JsonConvert.DeserializeObject<EventData>(cloudEvent.Data.ToString());

                log.LogInformation($"AttemptCompletionCode: {data.AttemptCompletionCode} RequestCompletionStatus: {data.RequestCompletionStatus}");

                if (!string.IsNullOrEmpty(data.AttemptCompletionCode))
                {
                    await _referralRepository.UpdateReferralOrdersStatusAsync(
                        data.RequestCorrelationToken, data.AttemptCompletionCode, $"Activity Time: {data.ActivityTime}; Request ID - Delivery ID: {data.RequestId} - {data.DeliveryId}; Originator Reference ID: {data.OriginatorReferenceId};\n{data.ActivityText}");
                }

                if (!string.IsNullOrEmpty(data.RequestCompletionStatus))
                {
                    await _referralRepository.UpdateReferralOrdersStatusAsync(
                        data.RequestCorrelationToken, data.RequestCompletionStatus, data.ActivityText);
                }

                if (AppointmentScheduledCompletionCode.Equals(data.RequestCompletionStatus, StringComparison.OrdinalIgnoreCase)
                    || AppointmentScheduledCompletionCode.Equals(data.AttemptCompletionCode, StringComparison.OrdinalIgnoreCase))
                {
                    log.LogInformation("Storing the appointment details into database.");

                    Common.Models.Appointment appointment = JsonConvert.DeserializeObject<Common.Models.Appointment>(data.Meta);

                    await _referralRepository.StoreAppointmentDetailsAsync(data.RequestCorrelationToken, appointment);

                    var requestToSend = BuildRequest(data, appointment);

                    await SendRequestAsync(data.RequestCorrelationToken, requestToSend, log);
                }
            }
            catch (Exception ex)
            {
                log.LogError($"Failed to process callback request. {ex + "\n" + ex.StackTrace}");

                throw;
            }
        }

        private static string ToTitleCase(string originalString)
        {
            var textInfoEn = new CultureInfo("en-US", false).TextInfo;
            originalString = textInfoEn.ToLower(originalString);
            return textInfoEn.ToTitleCase(originalString);
        }

        private static Request BuildRequest(EventData data, Common.Models.Appointment appointment)
        {
            return new Request
            {
                ConfigurationToken = "AppointmentManagementConfirmation",
                CorrelationToken = data.RequestCorrelationToken,
                Addresses = new List<Address>
                {
                    new()
                    {
                        Parameters = new Dictionary<string, string>
                        {
                            { "to", data.RequestAddressValue }
                        },
                        AddressPointer = "to",
                        CorrelationToken = data.AddressCorrelationToken,
                        TimeZone = null
                    }
                },
                Parameters = new Dictionary<string, string>
                {
                    { "providerName", appointment.ProviderDisplayName },
                    { "confirmationPageUrl", appointment.ConfirmationPageUrl },
                    { "appointmentDateTime", appointment.StartDateTime.ToString("s") },
                    { "streetName", ToTitleCase(appointment.DepartmentStreetName) },
                    { "city", ToTitleCase(appointment.DepartmentCity) },
                    { "state", appointment.DepartmentState },
                    { "zipCode", appointment.DepartmentZipCode },
                },
                Attachments = null
            };
        }

        private async Task SendRequestAsync(string referralId, Request requestToSend, ILogger<Listener> log)
        {
            using var client = await _ingestionClient.CreateClient();

            var response = await _ingestionClient.SendRequest(client, requestToSend);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseData = JsonConvert.DeserializeObject<Response>(content);

                await _referralRepository.UpdateReferralOrdersStatusAsync(
                    referralId,
                    ConfirmationSent,
                    responseData.RequestTrackingId);

                log.LogInformation(
                    "Communication Hub request has been submitted. (Correlation Token {Token}; Request ID: {RequestId}; Creation Time: {CreationTime}).",
                    responseData.CorrelationToken,
                    responseData.RequestTrackingId,
                    responseData.CreationTime);
            }
            else
            {
                await _referralRepository.UpdateReferralOrdersStatusAsync(
                    referralId,
                    ConfirmationFailed,
                    content);

                log.LogWarning(
                    "Communication Hub request submission has been failed (Status Code: {StatusCode}; Content: {Content}).",
                    response.StatusCode,
                    content);
            }
        }
    }
}
