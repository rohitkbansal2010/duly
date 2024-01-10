// <copyright file="ILogAnalyticsEntryBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Azure.Messaging;
using Duly.Clinic.Audit.Ingestion.Models;

namespace Duly.Clinic.Audit.Ingestion.Interfaces
{
    /// <summary>
    /// Builds <see cref="LogAnalyticsEntry" /> entity based on <see cref="CloudEvent" /> object.
    /// </summary>
    public interface ILogAnalyticsEntryBuilder
    {
        /// <summary>
        /// Builds <see cref="LogAnalyticsEntry"/> from <see cref="CloudEvent" />.
        /// </summary>
        /// <param name="cloudEvent">An instacne of <see cref="CloudEvent" /> object.</param>
        /// <returns>
        /// An instacne of <see cref="LogAnalyticsEntry" /> object.
        /// </returns>
        LogAnalyticsEntry BuildLogAnalyticsEntry(CloudEvent cloudEvent);
    }
}
