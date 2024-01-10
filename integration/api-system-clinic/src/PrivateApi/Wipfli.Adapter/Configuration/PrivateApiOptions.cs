// <copyright file="PrivateApiOptions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Wipfli.Adapter.Configuration
{
    /// <summary>
    /// Configuration options for private API from appSettings.
    /// </summary>
    public class PrivateApiOptions
    {
        /// <summary>
        /// Url to certificates storage.
        /// </summary>
        public string CertificateKeyVaultUrl { get; init; }

        /// <summary>
        /// Certificate name.
        /// </summary>
        public string CertificateName { get; init; }

        /// <summary>
        /// API endpoint base url.
        /// </summary>
        public string ApiBaseAddress { get; set; }

        /// <summary>
        /// API username.
        /// </summary>
        public string ApiUsername { get; init; }

        /// <summary>
        /// API password.
        /// </summary>
        public string ApiPassword { get; init; }

        /// <summary>
        /// API client ID.
        /// </summary>
        public string ApiClientId { get; init; }

        /// <summary>
        /// Bypass appointment creation when true.
        /// </summary>
        public bool BypassAppointmentCreation { get; init; }

        /// <summary>
        /// Timeout in sec.
        /// </summary>
        public double Timeout { get; init; } = 300;
    }
}