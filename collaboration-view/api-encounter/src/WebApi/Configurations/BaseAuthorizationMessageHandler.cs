// <copyright file="BaseAuthorizationMessageHandler.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Configurations
{
    public abstract class BaseAuthorizationMessageHandler<T> : DelegatingHandler
        where T : IHasScopes
    {
        private const string AuthorizationHeaderSchemaName = "Bearer";

        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly IOptionsMonitor<T> _optionsMonitor;

        protected BaseAuthorizationMessageHandler(ITokenAcquisition tokenAcquisition, IOptionsMonitor<T> optionsMonitor)
        {
            _tokenAcquisition = tokenAcquisition;
            _optionsMonitor = optionsMonitor;
        }

        private string[] Scopes => _optionsMonitor.CurrentValue.Scopes;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await GetAccessToken();

            request.Headers.Authorization = new AuthenticationHeaderValue(AuthorizationHeaderSchemaName, accessToken);

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<string> GetAccessToken()
        {
            string accessToken;
            try
            {
                accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes: Scopes);
            }
            catch (MsalUiRequiredException ex)
            {
                //It can happen
                //that when middle-tier API tries to get a token for the downstream API,
                //the token acquisition service throws the MsalUiRequiredException meaning
                //that the user on the client calling the middle-tier API needs to perform more actions such as multi-factor authentication.
                //Given that the middle-tier API isn't capable of doing interaction itself, this exception needs to be passed to the client.
                //To propagate back this exception to the client,
                //controller action code should catch the exception and call the ITokenAcquisition.ReplyForbiddenWithWwwAuthenticateHeader method.
                _tokenAcquisition.ReplyForbiddenWithWwwAuthenticateHeader(Scopes, ex);
                throw;
            }

            return accessToken;
        }
    }
}