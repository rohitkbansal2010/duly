// <copyright file="WipfliAuthorizationMessageHandler.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wipfli.Adapter.Configuration
{
    internal class WipfliAuthorizationMessageHandler : DelegatingHandler
    {
        private readonly IOptionsMonitor<PrivateApiOptions> _optionsMonitor;

        public WipfliAuthorizationMessageHandler(IOptionsMonitor<PrivateApiOptions> optionsMonitor)
        {
            _optionsMonitor = optionsMonitor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var options = _optionsMonitor.CurrentValue;

            string username = options.ApiUsername;
            string password = options.ApiPassword;

            string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", svcCredentials);
            request.Headers.Add("Epic-Client-ID", options.ApiClientId);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
