// <copyright file="IngestionClient.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Contracts.ConfigurationSettings;
using Duly.CollaborationView.Encounter.Api.Helpers;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <inheritdoc cref="IIngestionClient" />
    public class IngestionClient : IIngestionClient
    {
        private readonly IngestionSettings _ingestionSettings;
        private readonly Authentication _authenticationSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="IngestionClient" /> class.
        /// </summary>
        /// <param name="ingestionSettings">An instance of ingestion API configuration.</param>
        /// <param name="ingestionAuthentication">An instance of ingestion authentication configuration.</param>
        /// <param name="authenticationSettings">An instance of authentication configuration.</param>
        public IngestionClient(
            IOptions<IngestionSettings> ingestionSettings,
            IOptions<Authentication> authenticationSettings)
        {
            _ingestionSettings = ingestionSettings.Value;
            _authenticationSettings = authenticationSettings.Value;
        }

        public HttpResponseMessage SendRequest(Contracts.SendAfterVisitPdfSms requestResend)
        {
            var accessToken = AzureAdTokenHelper.GetAccessToken(
                _authenticationSettings.Instance,
                _authenticationSettings.TenantId,
                _ingestionSettings.ClientId,
                _ingestionSettings.ClientSecret,
                _ingestionSettings.IngestionScope);

            var requestUrl = _ingestionSettings.IngestionApiUrl;
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            httpClient.DefaultRequestHeaders.Add("subscription-key", $"{_ingestionSettings.SubscriptionKey}");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            string jsonRequestBody = JsonSerializer.Serialize(requestResend);
            request.Content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");
            var ingestionResponse = httpClient.SendAsync(request).Result;
            return ingestionResponse;
        }
    }
}