// <copyright file="IScheduleFollowUpAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// An adapter for working with "ScheduleFollowUp" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface IScheduleFollowUpAdapter
    {
        /// <summary>
        /// Finds matching items of <see cref="ScheduleFollowUp"/>.
        /// </summary>
        /// <param name="request">Criteria to search by.</param>
        /// <returns>Filtered items of <see cref="ScheduleFollowUp"/>.</returns>
        Task<int> PostScheduleFollowUpAsync(ScheduleFollowUp request);
    }
}