// <copyright file="ISendSMSAfterVisitedService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.SendSms"/> entity.
    /// </summary>
    public interface ISendSmsAfterVisitedService
    {
        /// <summary>
        /// Returns SMS sent response.
        /// </summary>
        /// <param name="request"><see cref="Contracts.SendAfterVisitPdfSms"/>SendSMS.</param>
        /// <returns>Returns status.</returns>
        public Task<SendAfterVisitPdfSms> SendAfterVisitPdfSms(SendSms request);
    }
}