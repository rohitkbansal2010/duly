// <copyright file="AuthorizationMessageHandler.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Configuration
{
    /// <summary>
    /// Handles auth calls to epic.
    /// </summary>
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly IClientAssertionCreator _clientAssertionCreator;
        private readonly ILogger<AuthorizationMessageHandler> _logger;

        private DulyEpicToken _token;

        public AuthorizationMessageHandler(
            IClientAssertionCreator clientAssertionCreator,
            AuditMessageHandler auditMessageHandler,
            ILogger<AuthorizationMessageHandler> logger)
        {
            InnerHandler = auditMessageHandler;
            _clientAssertionCreator = clientAssertionCreator;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await CheckTokenAsync(cancellationToken);

            var header = new AuthenticationHeaderValue(Constants.Bearer, _token?.AccessToken);
            request.Headers.Authorization = header;

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task CheckTokenAsync(CancellationToken cancellationToken)
        {
            if (IsTokenInvalid())
            {
                await _semaphore.WaitAsync(cancellationToken);
                try
                {
                    if (!cancellationToken.IsCancellationRequested && IsTokenInvalid())
                    {
                        _token = await ObtainToken();
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }

        private bool IsTokenInvalid()
        {
            return _token == null || !_token.IsValid();
        }

        private async Task<DulyEpicToken> ObtainToken()
        {
            var clientAssertion = await _clientAssertionCreator.GetClientAssertionAsync();
            var token = await _clientAssertionCreator.AuthenticationAsync(clientAssertion);

            if (token == null)
            {
                _logger.LogError("Epic Fhir Token is null");
            }
            else
            {
                _logger.LogDebug($"Epic token: {JsonConvert.SerializeObject(token)}");
            }

            return token;
        }

        protected override void Dispose(bool disposing)
        {
            using (_semaphore)
            {
            }

            base.Dispose(disposing);
        }
    }
}