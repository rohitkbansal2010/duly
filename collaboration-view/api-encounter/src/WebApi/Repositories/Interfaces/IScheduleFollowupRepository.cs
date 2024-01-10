// <copyright file="IScheduleFollowupRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Models.CheckOut.ScheduleFollowUp"/> model.
    /// </summary>
    public interface IScheduleFollowupRepository
    {
        Task<int> PostScheduleFollowup(ScheduleFollowUp request);

        Task<int> PostDataPostedToEpic(int id);
    }
}