// <copyright file="ServiceBusAuditProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Messaging.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Audit
{
    public class ServiceBusAuditProvider : IAuditProvider
    {
        private const string FailedToWriteAuditEntry = "Failed to write Audit entry.";
        private readonly IMessageProcessor<AuditLogEntry> _messageProcessor;
        private readonly ILogger<ServiceBusAuditProvider> _logger;

        public ServiceBusAuditProvider(
            IMessageProcessor<AuditLogEntry> messageProcessor,
            ILogger<ServiceBusAuditProvider> logger)
        {
            _messageProcessor = messageProcessor;
            _logger = logger;
        }

        public Task LogAsync(AuditLogEntry logEntry)
        {
            return SafeLogAsync(logEntry);
        }

        private async Task SafeLogAsync(AuditLogEntry logEntry)
        {
            try
            {
                await _messageProcessor.SendAsync(logEntry, logEntry.ClientId, logEntry.Type.ToString(), logEntry.Action.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, FailedToWriteAuditEntry);
            }
        }
    }
}
