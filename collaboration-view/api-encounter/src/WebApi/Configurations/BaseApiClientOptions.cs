// -----------------------------------------------------------------------
// <copyright file="BaseApiClientOptions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Duly.CollaborationView.Encounter.Api.Configurations
{
    /// <summary>
    /// Configuration options for the API clients.
    /// </summary>
    public abstract class BaseApiClientOptions : IHasScopes
    {
        /// <summary>
        /// Base address of the Clinic API.
        /// </summary>
        public string ApiBaseAddress { get; set; }

        /// <summary>
        /// subscription-key header value.
        /// </summary>
        public string SubscriptionKey { get; set; }

        /// <summary>
        /// Scopes.
        /// </summary>
        public string[] Scopes { get; set; }

        /// <summary>
        /// Timeout in sec.
        /// </summary>
        public double Timeout { get; set; } = 300;
    }
}