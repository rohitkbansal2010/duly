﻿// <copyright file="IReferralService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.ScheduleFollowUp"/> entity.
    /// </summary>
    public interface IReferralService
    {
        /// <summary>
        /// Returns referral response.
        /// </summary>
        /// <param name="request"><see cref="Contracts.ScheduleFollowUp"/>Referral.</param>
        /// <returns>Returns cvCheckOut_ID.</returns>
        Task<int> PostReferral(Contracts.ScheduleReferral request);

        /// <summary>
        /// Returns schedule followup response.
        /// </summary>
        /// <param name="id">id of Schedulefollowup.</param>
        /// <returns>Returns cvCheckOut_ID.</returns>
        Task<int> DataPostedToEpicAsync(int id);
    }
}