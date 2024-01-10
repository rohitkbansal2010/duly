// <copyright file="IngestionClient.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.OmniChannel.Orchestrator.Appointment.Common.Configuration;
using Duly.OmniChannel.Orchestrator.Appointment.Common.Interfaces;
using Duly.OmniChannel.Orchestrator.Appointment.Common.Models;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Duly.OmniChannel.Orchestrator.Appointment.Common.Services
{
    /// <inheritdoc cref="IIngestionClient" />
    public class IngestionClient : IIngestionClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly IngestionSettings _ingestionSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="IngestionClient" /> class.
        /// </summary>
        /// <param name="httpClientFactory">An instance of HTTP client factory.</param>
        /// <param name="tokenAcquisition">An instance of token acquisition factory.</param>
        /// <param name="ingestionSettings">An instance of ingestion API configuration.</param>
        public IngestionClient(
            IHttpClientFactory httpClientFactory,
            ITokenAcquisition tokenAcquisition,
            IOptions<IngestionSettings> ingestionSettings)
        {
            _httpClientFactory = httpClientFactory;
            _tokenAcquisition = tokenAcquisition;
            _ingestionSettings = ingestionSettings.Value;
        }

        /// <inheritdoc />
        public async Task<HttpClient> CreateClient(string scope, string subscriptionKey = null)
        {
            var result = _httpClientFactory.CreateClient();

            result.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    Constants.Bearer,
                    await _tokenAcquisition.GetAccessTokenForAppAsync(scope));

            if (!string.IsNullOrEmpty(subscriptionKey))
            {
                result.DefaultRequestHeaders.Add("subscription-key", subscriptionKey);
            }

            return result;
        }

        /// <inheritdoc />
        public async Task<HttpClient> CreateClient()
        {
            return await CreateClient(_ingestionSettings.IngestionScope, _ingestionSettings.SubscriptionKey);
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> SendRequest(HttpClient client, Request request, CancellationToken cancellationToken = default)
        {
            return await client.PostAsJsonAsync(_ingestionSettings.IngestionApiUrl, request, cancellationToken).ConfigureAwait(false);
        }
    }
}
