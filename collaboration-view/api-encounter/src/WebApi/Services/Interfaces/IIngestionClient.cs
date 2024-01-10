// <copyright file="IIngestionClient.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Net.Http;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// A client of Communication Hub Ingestion API.
    /// </summary>
    public interface IIngestionClient
    {
        /// <summary>
        /// Sends request to Communication Hub Ingestion API.
        /// </summary>
        /// <param name="requestResend">A request for sending.</param>
        /// <returns>
        /// A response after execution of ingestion API.
        /// </returns>
        HttpResponseMessage SendRequest(Contracts.SendAfterVisitPdfSms requestResend);
    }
}