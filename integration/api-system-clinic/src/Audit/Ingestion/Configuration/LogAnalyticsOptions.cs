// <copyright file="LogAnalyticsOptions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.Clinic.Audit.Ingestion.Configuration
{
    /// <summary>
    /// Configuration options for the Log Analytics components.
    /// </summary>
    public class LogAnalyticsOptions
    {
        /// <summary>
        /// The type of log entry that will be written to the Azure Log Analytics collector.
        /// </summary>
        public string LogAnalyticsEntryType { get; set; }
    }
}