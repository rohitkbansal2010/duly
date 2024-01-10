// <copyright file="AzureAdTokenHelper.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Duly.CollaborationView.Encounter.Api.Helpers
{
    /// <summary>
    /// Returns access token.
    /// <param name="intance">Instance.</param>
    /// <param name="tenantId">Tenant id.</param>
    /// <param name="clientId">Client id.</param>
    /// <param name="clientSecret">Client secret.</param>
    /// </summary>
    public static class AzureAdTokenHelper
    {
        public static string GetAccessToken(string intance, string tenantId, string clientId, string clientSecret, string scope)
        {
            AuthenticationContext context = new AuthenticationContext($"{intance}{tenantId}");
            ClientCredential clientCredential = new ClientCredential($"{clientId}", $"{clientSecret}");
            var token = context.AcquireTokenAsync($"{scope}", clientCredential).Result.AccessToken;
            return token;
        }
    }
}
