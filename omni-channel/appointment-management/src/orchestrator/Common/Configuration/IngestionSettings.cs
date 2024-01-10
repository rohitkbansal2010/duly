// <copyright file="IngestionSettings.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.OmniChannel.Orchestrator.Appointment.Common.Configuration
{
    /// <summary>
    /// Defines a model of application settings.
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
    }
}
