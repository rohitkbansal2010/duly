// <copyright file="Invitation.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

#if DEBUG
using Bogus;
#endif

using Duly.OmniChannel.Orchestrator.Appointment.Common.Interfaces;
using Duly.OmniChannel.Orchestrator.Appointment.Common.Models;
using Hangfire;
using Hangfire.Console.Extensions;
using Hangfire.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Duly.OmniChannel.Orchestrator.Appointment.Workflow
{
    /// <summary>
    /// Represents the workflow to prepare and send invitation requests.
    /// </summary>
    public class Invitation
    {
        private const string InvitationSent = "INVITATION.SENT";
        private const string InvitationFailed = "INVITATION.FAILED";
        private const string ReferralSimulationEnabled = "ApplicationSettings:ReferralSimulation:Enabled";
        private const string InvitationConfigurationToken = "ApplicationSettings:InvitationConfigurationToken";

        private readonly ILogger<Invitation> _logger;
        private readonly IIngestionClient _ingestionClient;
        private readonly PerformingContext _performingContext;
        private readonly IJobManager _jobManager;
        private readonly IJobCancellationToken _jobCancellationToken;
        private readonly IProgressBarFactory _progressBarFactory;
        private readonly IReferralRepository _referralRepository;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Invitation" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="ingestionClient">An instance of ingestion client class.</param>
        /// <param name="jobCancellationToken">An instance of job cancellation token.</param>
        /// <param name="performingContext">An instance of Hangfire server performing context.</param>
        /// <param name="jobManager">An instance of Hangfire job manager.</param>
        /// <param name="progressBarFactory">An instance of Hangfire progress bar factory.</param>
        /// <param name="referralRepository">An instance of <see cref="IReferralRepository" /> class.</param>
        /// <param name="configuration">An instance of the configuration object.</param>
        public Invitation(
            ILogger<Invitation> logger,
            IIngestionClient ingestionClient,
            IJobCancellationToken jobCancellationToken,
            PerformingContext performingContext,
            IJobManager jobManager,
            IProgressBarFactory progressBarFactory,
            IReferralRepository referralRepository,
            IConfiguration configuration)
        {
            _logger = logger;
            _ingestionClient = ingestionClient;
            _performingContext = performingContext;
            _jobManager = jobManager;
            _jobCancellationToken = jobCancellationToken;
            _progressBarFactory = progressBarFactory;
            _referralRepository = referralRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// Executes job for sending appointment management invitation.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        [JobDisplayName("appointment-management-invitation-job")]
        [AutomaticRetry(Attempts = 0)]
        [DisableConcurrentExecution(timeoutInSeconds: 60 * 60 * 3)]
        public async Task RunAsync()
        {
            _logger.LogInformation("Job Id: {JobId}.", _performingContext.BackgroundJob.Id);

            var recordSet = await GetReferralOrdersAsync();

            if (recordSet is null || !recordSet.Any())
            {
                _logger.LogInformation("No referral orders to send.");
            }
            else
            {
                using var client = await _ingestionClient.CreateClient();

                var progress = _progressBarFactory.Create();

                foreach (var (referralOrder, index) in recordSet.Select((value, index) => (value, index)))
                {
                    progress.SetValue(index + 1);

                    var request = BuildRequest(referralOrder);

                    await SendRequestAsync(client, request, referralOrder);
                }
            }
        }

        private async Task<IEnumerable<ReferralOrder>> GetReferralOrdersAsync()
        {
#if DEBUG
            IEnumerable<ReferralOrder> recordSet;

            var simulation = _configuration.GetValue<bool>(ReferralSimulationEnabled);

            if (simulation)
            {
                recordSet =
                    new Faker<ReferralOrder>()
                        .RuleFor(o => o.ReferralId, f => f.Random.AlphaNumeric(8))
                        .RuleFor(o => o.PatientId, f => f.Random.AlphaNumeric(5))
                        .RuleFor(o => o.PatientName, f => f.Person.FullName)
                        .RuleFor(o => o.PatientDateOfBirth, f => f.Person.DateOfBirth)
                        .RuleFor(o => o.PatientPhone, f => f.Phone.PhoneNumber())
                        .RuleFor(o => o.SpecialtyId, f => f.Random.AlphaNumeric(5))
                        .RuleFor(o => o.Specialty, f => f.Name.JobTitle())
                        .RuleFor(o => o.ProviderExternalId, f => f.Random.AlphaNumeric(5))
                        .RuleFor(o => o.ProviderName, f => f.Name.FullName())
                        .RuleFor(o => o.ProviderPhotoUrl, f => f.Internet.Avatar())
                        .Generate(_configuration.GetValue<int>("ApplicationSettings:ReferralSimulation:Count"));
            }
            else
            {
                recordSet = await _referralRepository.GetReferralOrdersForDeliveryAsync();
            }
#else
            var recordSet = await _referralRepository.GetReferralOrdersForDeliveryAsync();
#endif
            return recordSet;
        }

        private async Task SendRequestAsync(HttpClient client, Request request, ReferralOrder referralOrder)
        {
            var response = await _ingestionClient.SendRequest(client, request, _jobCancellationToken.ShutdownToken);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseData = JsonConvert.DeserializeObject<Response>(content);

                await _referralRepository.UpdateReferralOrdersStatusAsync(
                    referralOrder.ReferralId,
                    InvitationSent,
                    responseData.RequestTrackingId);

                _logger.LogInformation(
                    "Communication Hub request has been submitted. (Correlation Token {Token}; Request ID: {RequestId}; Creation Time: {CreationTime}).",
                    responseData.CorrelationToken,
                    responseData.RequestTrackingId,
                    responseData.CreationTime);
            }
            else
            {
                await _referralRepository.UpdateReferralOrdersStatusAsync(
                    referralOrder.ReferralId,
                    InvitationFailed,
                    content);

                _logger.LogWarning(
                    "Communication Hub request submission has been failed (Status Code: {StatusCode}; Content: {Content}).",
                    response.StatusCode,
                    content);
            }
        }

        private Request BuildRequest(ReferralOrder referralOrder)
        {
            var request = new Request
            {
                ConfigurationToken = _configuration.GetValue<string>(InvitationConfigurationToken),
                CorrelationToken = referralOrder.ReferralId,
                Addresses = new List<Address>
                {
                    new()
                    {
                        Parameters = new Dictionary<string, string>
                        {
                            { "to", referralOrder.PatientPhone }
                        },
                        AddressPointer = "to",
                        CorrelationToken = referralOrder.PatientId,
                        TimeZone = null
                    }
                },
                Parameters = new Dictionary<string, string>
                {
                    { "patientName", referralOrder.PatientName },
                    { "patientDateOfBirth", referralOrder.PatientDateOfBirth.ToString("d") },
                    { "specialityId", referralOrder.SpecialtyId },
                    { "speciality", referralOrder.Specialty },
                    { "referredByProviderId", referralOrder.ProviderExternalId },
                    { "referredByProviderName", referralOrder.ProviderName },
                    { "referredByProviderPhotoUrl", referralOrder.ProviderPhotoUrl },
                },
                Attachments = null
            };
            return request;
        }
    }
}
