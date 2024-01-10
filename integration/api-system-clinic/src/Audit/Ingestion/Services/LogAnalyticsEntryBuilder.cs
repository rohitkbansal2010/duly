// <copyright file="LogAnalyticsEntryBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Azure.Messaging;
using Duly.Clinic.Audit.Ingestion.Exceptions;
using Duly.Clinic.Audit.Ingestion.Interfaces;
using Duly.Clinic.Audit.Ingestion.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Duly.Clinic.Audit.Ingestion.Services
{
    /// <inheritdoc cref="ILogAnalyticsEntryBuilder" />
    public class LogAnalyticsEntryBuilder : ILogAnalyticsEntryBuilder
    {
        private const string UserFieldName = "User";
        private const string MetaFieldName = "Meta";

        /// <inheritdoc />
        public LogAnalyticsEntry BuildLogAnalyticsEntry(CloudEvent cloudEvent)
        {
            if (cloudEvent is null)
                throw new ArgumentNullException(nameof(cloudEvent));

            var cloudEventData = JsonConvert.DeserializeObject<CloudEventData>(cloudEvent.Data?.ToString() ?? string.Empty);

            if (cloudEventData is null)
                throw new LogAnalyticsEntryBuilderException("The cloud event data field is null.");

            return
                new LogAnalyticsEntry
                {
                    Originator = cloudEventData.Originator,
                    EventTime = cloudEventData.EventTime,
                    CorrelationToken = cloudEventData.CorrelationToken,
                    ClientId = cloudEventData.ClientId,
                    Type = cloudEventData.Type,
                    Action = cloudEventData.Action,
                    ServiceHost = cloudEventData.ServiceHost,
                    ServiceName = cloudEventData.ServiceName,
                    ServiceVersion = cloudEventData.ServiceVersion,
                    Operation = cloudEventData.Operation,
                    User = GetFieldAsJson(cloudEventData.AdditionalData, UserFieldName),
                    Meta = GetFieldAsJson(cloudEventData.AdditionalData, MetaFieldName)
                };
        }

        private static string GetFieldAsJson(IDictionary<string, JToken> additionalData, string fieldName)
        {
            if (additionalData is null)
                return null;

            return
                additionalData.TryGetValue(fieldName, out var jsonToken)
                    ? jsonToken.ToString(Formatting.None)
                    : null;
        }
    }
}
