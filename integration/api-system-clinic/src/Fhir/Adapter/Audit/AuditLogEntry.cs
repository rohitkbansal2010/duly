// <copyright file="AuditLogEntry.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Duly.Clinic.Fhir.Adapter.Audit
{
    /// <summary>
    /// Information about event that requires to be audited.
    /// </summary>
    public class AuditLogEntry
    {
        /// <summary>
        /// Creator of this entry.
        /// </summary>
        public string Originator { get; init; }

        /// <summary>
        /// DateTime when event happened.
        /// </summary>
        public DateTime EventTime { get; init; }

        /// <summary>
        /// Correlation id, token, etc.
        /// </summary>
        public string CorrelationToken { get; init; }

        /// <summary>
        /// User who performed action.
        /// </summary>
        public AuditUser User { get; init; }

        /// <summary>
        /// Id of app that performed action.
        /// </summary>
        public string ClientId { get; init; }

        /// <summary>
        /// Read, Write etc.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public AuditEntryAction Action { get; init; }

        /// <summary>
        /// Type of event.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public AuditEntryType Type { get; init; }

        /// <summary>
        /// Service used e.g. Appointments, Medication, etc.
        /// </summary>
        public string ServiceName { get; init; }

        /// <summary>
        /// Version of service.
        /// </summary>
        public string ServiceVersion { get; init; }

        /// <summary>
        /// Url to instance of DataProvider.
        /// </summary>
        public string ServiceHost { get; init; }

        /// <summary>
        /// Type of performed operation e.g. Search, Read, Create etc.
        /// </summary>
        public string Operation { get; init; }

        /// <summary>
        /// Additional data.
        /// </summary>
        public AuditMeta Meta { get; init; }
    }
}