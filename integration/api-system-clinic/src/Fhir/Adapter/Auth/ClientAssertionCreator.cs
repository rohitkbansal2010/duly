// <copyright file="ClientAssertionCreator.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Azure.Identity;
using Azure.Security.KeyVault.Keys.Cryptography;
using Duly.Clinic.Fhir.Adapter.Auth.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Auth
{
    /// <summary>
    /// <inheritdoc cref="IClientAssertionCreator"/>
    /// </summary>
    internal class ClientAssertionCreator : IClientAssertionCreator
    {
        private readonly IOptionsMonitor<DulyRestApiClientAuthSettings> _dulyRestApiClientAuthSettingsOptionsMonitor;
        private readonly IOptionsMonitor<FhirAuthSettings> _fhirAuthSettingsOptionsMonitor;
        private readonly ILogger<ClientAssertionCreator> _logger;
        public ClientAssertionCreator(
            IOptionsMonitor<DulyRestApiClientAuthSettings> dulyRestApiClientAuthSettingsOptionsMonitor,
            IOptionsMonitor<FhirAuthSettings> fhirAuthSettingsOptionsMonitor,
            ILogger<ClientAssertionCreator> logger)
        {
            _dulyRestApiClientAuthSettingsOptionsMonitor = dulyRestApiClientAuthSettingsOptionsMonitor;
            _fhirAuthSettingsOptionsMonitor = fhirAuthSettingsOptionsMonitor;
            _logger = logger;
        }

        private DulyRestApiClientAuthSettings DulyRestApiClientAuthSettings =>
            _dulyRestApiClientAuthSettingsOptionsMonitor.CurrentValue;

        private FhirAuthSettings FhirAuthSettings => _fhirAuthSettingsOptionsMonitor.CurrentValue;

        public async Task<string> GetClientAssertionAsync()
        {
            var token = JwtBuilder.BuildToken(FhirAuthSettings.ClientId, FhirAuthSettings.TokenUrl);
            var signature = await BuildSignature(token);
            var client_assertion_value = JwtBuilder.Combine(token, signature);
            return client_assertion_value;
        }

        public async Task<DulyEpicToken> AuthenticationAsync(string client_assertion_value)
        {
            var client = new RestClient(FhirAuthSettings.TokenUrl)
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");

            request.AddObject(JwtBuilder.BuildPayload(client_assertion_value));

            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                LogError(response);
            }

            return JsonConvert.DeserializeObject<DulyEpicToken>(response.Content);
        }

        private async Task<string> BuildSignature(string token)
        {
            var rsaCryptoClient = GetRsaCryptoClient();

            var hashedToken = JwtBuilder.HashToken(token);
            var rsaSignResult = await rsaCryptoClient.SignAsync(JwtBuilder.Algorithm, hashedToken);
            var signature = Base64UrlEncoder.Encode(rsaSignResult.Signature);
            return signature;
        }

        private CryptographyClient GetRsaCryptoClient()
        {
            var keyId = new Uri(DulyRestApiClientAuthSettings.VaultKeyIdUrl);
            var credential = new DefaultAzureCredential();
            var rsaCryptoClient = new CryptographyClient(keyId, credential);
            return rsaCryptoClient;
        }

        private void LogError(IRestResponse response)
        {
            //Set up the information message with the URL,
            //the status code, and the parameters.
            var info = $"Request failed with status code {response.StatusCode}, content: {response.Content}";

            //Acquire the actual exception
            Exception ex;
            if (response.ErrorException != null)
            {
                ex = response.ErrorException;
            }
            else
            {
                ex = new Exception(info);
                info = string.Empty;
            }

            //Log the exception and info message
            _logger.LogError(ex, info);
        }
    }
}