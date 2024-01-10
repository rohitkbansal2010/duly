// <copyright file="AuditMessageHandler.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Audit;
using Duly.Clinic.Fhir.Adapter.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Configuration
{
    /// <summary>
    /// Handles Audit of calls to epic.
    /// </summary>
    public class AuditMessageHandler : HttpClientHandler
    {
        private const string FailedAudit = "Failed To write audit";
        private const string Operation = "Search";
        private const string ApiSystemClinic = "api-system-clinic";
        private readonly IAuditProvider _auditProvider;
        private readonly IAuditAccountIdentityService _context;
        private readonly ILogger<AuditMessageHandler> _logger;

        public AuditMessageHandler(
            IAuditProvider auditProvider,
            ILogger<AuditMessageHandler> logger,
            IAuditAccountIdentityService context)
        {
            _auditProvider = auditProvider;
            _logger = logger;
            _context = context;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LogAuditRequest(request);
            try
            {
                var response = await base.SendAsync(request, cancellationToken);
                LogAuditResponse(response);
                return response;
            }
            catch (Exception ex)
            {
                LogAuditException(request, ex);
                throw;
            }
        }

        private static string GerServiceHost(Uri requestUri)
        {
            return $"{requestUri.Host}/{requestUri.Segments[1]}".TrimEnd('/');
        }

        private static string GetServiceVersion(IReadOnlyList<string> segments)
        {
            return segments[^2].TrimEnd('/');
        }

        private static string GetServiceName(IReadOnlyList<string> segments)
        {
            return segments[^1].TrimEnd('/');
        }

        private void LogExceptionIfNeeded(Task auditTask)
        {
            if (auditTask.Exception is not null)
            {
                _logger.LogError(auditTask.Exception, FailedAudit);
            }
        }

        private void LogAuditResponse(HttpResponseMessage response)
        {
            var responseEvent = PopulateEntry(response.RequestMessage, AuditEntryType.Response, new AuditMeta
            {
                ResponseCode = response.StatusCode.ToString()
            });

            FireAndForgetAuditLog(responseEvent);
        }

        private void LogAuditRequest(HttpRequestMessage request)
        {
            var requestEvent = PopulateEntry(request, AuditEntryType.Request);
            FireAndForgetAuditLog(requestEvent);
        }

        private void LogAuditException(HttpRequestMessage request, Exception exception)
        {
            var exceptionEvent = PopulateEntry(request, AuditEntryType.Exception, new AuditMeta
            {
                ExceptionMessage = exception.Message
            });

            FireAndForgetAuditLog(exceptionEvent);
        }

        private void FireAndForgetAuditLog(AuditLogEntry logEntry)
        {
            _auditProvider.LogAsync(logEntry).ContinueWith(LogExceptionIfNeeded, TaskContinuationOptions.OnlyOnFaulted);
        }

        private AuditLogEntry PopulateEntry(HttpRequestMessage request, AuditEntryType auditEntryType, AuditMeta meta = null)
        {
            var requestUri = request.RequestUri;
            return new AuditLogEntry
            {
                Originator = ApiSystemClinic,
                EventTime = DateTime.UtcNow,
                CorrelationToken = _context.GetXCorrelationId().ToString(),
                User = _context.GetUser(),
                ClientId = _context.GetAppId(),
                Action = AuditEntryAction.Read,
                ServiceName = GetServiceName(requestUri.Segments),
                ServiceVersion = GetServiceVersion(requestUri.Segments),
                ServiceHost = GerServiceHost(requestUri),
                Operation = Operation,
                Type = auditEntryType,
                Meta = meta
            };
        }
    }
}