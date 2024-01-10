// <copyright file="CloudEventLogAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Azure.Messaging;
using Duly.Clinic.Audit.Ingestion.Configuration;
using Duly.Clinic.Audit.Ingestion.Exceptions;
using Duly.Clinic.Audit.Ingestion.Interfaces;
using LogAnalytics.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Duly.Clinic.Audit.Ingestion.Services
{
    /// <inheritdoc cref="ICloudEventLogAdapter" />
    public class CloudEventLogAdapter : ICloudEventLogAdapter
    {
        private readonly ILogAnalyticsEntryBuilder _logAnalyticsEntryBuilder;
        private readonly ILogAnalyticsClient _logAnalyticsClient;
        private readonly IOptionsMonitor<LogAnalyticsOptions> _logAnalyticsOptionsMonitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudEventLogAdapter" /> class.
        /// </summary>
        /// <param name="logAnalyticsEntryBuilder">An instance of <see cref="ILogAnalyticsEntryBuilder" /> class.</param>
        /// <param name="logAnalyticsClient">An instance of <see cref="ILogAnalyticsClient" /> class.</param>
        /// <param name="logAnalyticsOptionsMonitor">An instance of <see cref="IOptionsMonitor{LogAnalyticsOptions}" /> class.</param>
        public CloudEventLogAdapter(
            ILogAnalyticsEntryBuilder logAnalyticsEntryBuilder,
            ILogAnalyticsClient logAnalyticsClient,
            IOptionsMonitor<LogAnalyticsOptions> logAnalyticsOptionsMonitor)
        {
            _logAnalyticsEntryBuilder = logAnalyticsEntryBuilder;
            _logAnalyticsClient = logAnalyticsClient;
            _logAnalyticsOptionsMonitor = logAnalyticsOptionsMonitor;
        }

        /// <inheritdoc />
        public async Task<bool> SendCloudEventAsync(CloudEvent cloudEvent, ILogger logger)
        {
            try
            {
                var entry = _logAnalyticsEntryBuilder.BuildLogAnalyticsEntry(cloudEvent);

                var logType = _logAnalyticsOptionsMonitor.CurrentValue?.LogAnalyticsEntryType
                    ?? throw new LogAnalyticsEntryBuilderException("The LogAnalyticsEntryType is not specified.");

                await _logAnalyticsClient.SendLogEntry(entry, logType, cloudEvent.Source);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"The cloud event [{cloudEvent.Id}] was not sent.");

                return false;
            }
        }
    }
}
