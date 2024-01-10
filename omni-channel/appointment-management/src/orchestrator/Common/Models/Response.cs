// <copyright file="Response.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Newtonsoft.Json;
using System;

namespace Duly.OmniChannel.Orchestrator.Appointment.Common.Models
{
    /// <summary>
    /// Represents a model of successful response contract.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Gets or sets an unique identifier of request from submitter.
        /// </summary>
        public string CorrelationToken { get; set; }

        /// <summary>
        /// Gets or sets a created request tracking identifier.
        /// </summary>
        public string RequestTrackingId { get; set; }

        /// <summary>
        /// Gets or sets date and time of created request.
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Gets or sets additional info related to the result of performing a request (optional).
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Meta { get; set; }
    }
}