// <copyright file="Address.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Annotations.Json;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Duly.OmniChannel.Orchestrator.Appointment.Common.Models
{
    /// <summary>
    /// Represents a model of request address data.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Gets or sets external ID of the address.
        /// </summary>
        public string CorrelationToken { get; set; }

        /// <summary>
        /// Gets or sets recipient time zone.
        /// </summary>
        /// <example>
        /// A value can be passed as scalar "timeZone": "US/Eastern".
        /// </example>
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets or sets a collection of address parameters.
        /// </summary>
        [JsonConverter(typeof(PolytypicParametersObjectConverter.WithNestedJsonSupport))]
        public IDictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// Gets or sets an address pointer.
        /// Contains the name of a parameter containing actual address value.
        /// </summary>
        public string AddressPointer { get; set; }
    }
}
