// <copyright file="Authentication.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    /// <summary>
    /// Defines a model of authentication settings.
    /// </summary>
    public class Authentication
    {
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