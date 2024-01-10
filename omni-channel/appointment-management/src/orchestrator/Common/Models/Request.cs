// <copyright file="Request.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Annotations.Json;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Duly.OmniChannel.Orchestrator.Appointment.Common.Models
{
    /// <summary>
    /// A model of communication hub request object.
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Gets or sets an unique token of configuration.
        /// </summary>
        public string ConfigurationToken { get; set; }

        /// <summary>
        /// Gets or sets an unique identifier of the request from workflow.
        /// </summary>
        public string CorrelationToken { get; set; }

        /// <summary>
        /// Gets or sets a priority order of the request.
        /// </summary>
        public byte? Priority { get; set; }

        /// <summary>
        /// Gets or sets a timeout of delivery processing in minutes.
        /// </summary>
        public int? DeliveryTimeout { get; set; }

        /// <summary>
        /// Gets or sets a collection of request parameters.
        /// </summary>
        [JsonConverter(typeof(PolytypicParametersObjectConverter.WithNestedJsonSupport))]
        public IDictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// Gets or sets a collection of request addresses.
        /// </summary>
        public IEnumerable<Address> Addresses { get; set; }

        /// <summary>
        /// Gets or sets a collection of request attachments.
        /// </summary>
        public IEnumerable<Attachment> Attachments { get; set; }
    }
}