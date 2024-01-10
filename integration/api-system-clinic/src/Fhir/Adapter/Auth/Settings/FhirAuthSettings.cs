// <copyright file="FhirAuthSettings.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.Clinic.Fhir.Adapter.Auth.Settings
{
    /// <summary>
    /// Settings for auth in Epic.
    /// </summary>
    public class FhirAuthSettings
    {
        public string TokenUrl { get; set; }
        public string ClientId { get; set; }
    }
}
