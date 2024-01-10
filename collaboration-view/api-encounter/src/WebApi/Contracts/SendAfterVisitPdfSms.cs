// <copyright file="SendAfterVisitPdfSms.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    /// <summary>
    /// A model of communication hub request object.
    /// </summary>
    public class SendAfterVisitPdfSms
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
        //[JsonConverter(typeof(PolytypicParametersObjectConverter.WithNestedJsonSupport))]
        public IDictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// Gets or sets a collection of request addresses.
        /// </summary>
        public IEnumerable<AddressModelSms> Addresses { get; set; }
    }

    public class AddressModelSms
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
       // [JsonConverter(typeof(PolytypicParametersObjectConverter.WithNestedJsonSupport))]
        public IDictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// Gets or sets an address pointer.
        /// Contains the name of a parameter containing actual address value.
        /// </summary>
        public string AddressPointer { get; set; }
    }
}