// <copyright file="ICloudEventLogAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Azure.Messaging;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Duly.Clinic.Audit.Ingestion.Interfaces
{
    /// <summary>
    /// Logs a <see cref="CloudEvent"/> to an Azure Log Analytics workspace.
    /// </summary>
    public interface ICloudEventLogAdapter
    {
        /// <summary>
        /// Sends a <see cref="CloudEvent" /> to an Azure Log Analytics workspace.
        /// </summary>
        /// <param name="cloudEvent">An instacne of <see cref="CloudEvent" /> object.</param>
        /// <param name="logger">An instance of <see cref="ILogger" /> class.</param>
        /// <returns>
        /// <c>true</c> if the event was successfully sent; otherwise <c>false</c>.
        /// </returns>
        Task<bool> SendCloudEventAsync(CloudEvent cloudEvent, ILogger logger);
    }
}
