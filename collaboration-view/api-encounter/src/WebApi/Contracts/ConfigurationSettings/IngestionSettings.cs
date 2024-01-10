// <copyright file="IngestionSettings.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Contracts.ConfigurationSettings
{
    /// <summary>
    /// Defines a model of ingestion settings.
    /// </summary>
    public class IngestionSettings
    {
        /// <summary>
        /// Gets or sets an APIM subscription key of Ingestion API.
        /// </summary>
        public string SubscriptionKey { get; set; }

        /// <summary>
        /// Gets or sets an URL of Communication Hub Ingestion API.
        /// </summary>
        public string IngestionApiUrl { get; set; }

        /// <summary>
        /// Gets or sets a client scope required for authentication.
        /// </summary>
        public string IngestionScope { get; set; }

        /// <summary>
        /// Gets or sets an instance for authentication.
        /// </summary>
        public string Instance { get; set; }

        /// <summary>
        /// Gets or sets tenant id for authentication.
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets a client id required for authentication.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets a client secret required for authentication.
        /// </summary>
        public string ClientSecret { get; set; }
    }
}