// <copyright file="DulyRestApiClientAuthSettings.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.Clinic.Fhir.Adapter.Auth.Settings
{
    /// <summary>
    /// Auth settings for Duly Rest Api Client.
    /// </summary>
    public class DulyRestApiClientAuthSettings
    {
        /// <summary>
        /// Vault for signing.
        /// </summary>
        public string VaultKeyIdUrl { get; set; }
    }
}
