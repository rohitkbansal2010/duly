// <copyright file="IRecommendedProviderAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// An adapter for working with "RecommendedProvider" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface IRecommendedProviderAdapter
    {
        /// <summary>
        /// Finds matching items of <see cref="RecommendedProvider"/> by referral id.
        /// </summary>
        /// <param name="referralId">Referral identifier.</param>
        /// <returns>Items of <see cref="RecommendedProvider"/>.</returns>
        public Task<IEnumerable<RecommendedProvider>> FindRecommendedProvidersByReferralIdAsync(string referralId);

        /// <summary>
        /// Finds first matching item of <see cref="RecommendedProvider"/> by referral id.
        /// Or null if not found.
        /// </summary>
        /// <param name="referralId">Referral identifier.</param>
        /// <returns>Items of <see cref="RecommendedProvider"/>.</returns>
        public Task<RecommendedProvider> FindFirstRecommendedProviderByReferralIdAsync(string referralId);
    }
}
