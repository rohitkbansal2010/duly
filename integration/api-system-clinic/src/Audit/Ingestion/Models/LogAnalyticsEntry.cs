// <copyright file="LogAnalyticsEntry.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Clinic.Audit.Ingestion.Models
{
    public class LogAnalyticsEntry
    {
        public DateTime EventTime { get; set; }

        public string Originator { get; set; }

        public string CorrelationToken { get; set; }

        public string User { get; set; }

        public string ClientId { get; set; }

        public string Action { get; set; }

        public string Type { get; set; }

        public string ServiceName { get; set; }

        public string ServiceVersion { get; set; }

        public string ServiceHost { get; set; }

        public string Operation { get; set; }

        public string Meta { get; set; }
    }
}
