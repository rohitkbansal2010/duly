// <copyright file="DulyEpicToken.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Newtonsoft.Json;
using System;

namespace Duly.Clinic.Fhir.Adapter.Auth
{
    public class DulyEpicToken
    {
        private readonly DateTime _objectCreationDateTime = DateTime.Now;

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        public bool IsValid()
        {
            return AccessToken != null && DateTime.Now < _objectCreationDateTime.AddSeconds(ExpiresIn);
        }
    }
}
