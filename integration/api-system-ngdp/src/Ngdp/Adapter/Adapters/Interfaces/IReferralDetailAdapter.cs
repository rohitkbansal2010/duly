// <copyright file="IReferralDetailAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// An adapter for working with "ReferralDetail" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface IReferralDetailAdapter
    {
        /// <summary>
        /// Finds matching items of <see cref="ReferralDetail"/>.
        /// </summary>
        /// <param name="request">Criteria to search by.</param>
        /// <returns>Filtered items of <see cref="ReferralDetail"/>.</returns>
        Task<int> PostReferralDetailAsync(ReferralDetail request);
    }
}