// <copyright file="IIngestionClient.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.OmniChannel.Orchestrator.Appointment.Common.Models;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Duly.OmniChannel.Orchestrator.Appointment.Common.Interfaces
{
    /// <summary>
    /// A client of Communication Hub Ingestion API.
    /// </summary>
    public interface IIngestionClient
    {
        /// <summary>
        /// Creates a client of Communication Hub Ingestion API.
        /// </summary>
        /// <param name="scope">A client scope required for authentication.</param>
        /// <param name="subscriptionKey">An APIM subscription key of Ingestion API.</param>
        /// <returns>
        /// An instance of created Communication Hub Ingestion API client.
        /// </returns>
        Task<HttpClient> CreateClient(string scope, string subscriptionKey);

        /// <summary>
        /// Creates a client of Communication Hub Ingestion API base on default configuration.
        /// </summary>
        /// <returns>
        /// An instance of created Communication Hub Ingestion API client.
        /// </returns>
        Task<HttpClient> CreateClient();

        /// <summary>
        /// Sends request to Communication Hub Ingestion API.
        /// </summary>
        /// <param name="client">An instance of HTTP client.</param>
        /// <param name="request">A request for sending.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>
        /// A response after execution of ingestion API.
        /// </returns>
        Task<HttpResponseMessage> SendRequest(HttpClient client, Request request, CancellationToken cancellationToken = default);
    }
}
