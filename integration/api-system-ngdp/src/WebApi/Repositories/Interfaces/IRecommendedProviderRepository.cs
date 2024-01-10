// <copyright file="IRecommendedProviderRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces
{
    public interface IRecommendedProviderRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="RecommendedProvider"/> which match with the referral id.
        /// </summary>
        /// <param name="referralId">Referral identifier.</param>
        /// <returns>Items of <see cref="RecommendedProvider"/>.</returns>
        public Task<IEnumerable<RecommendedProvider>> GetRecommendedProvidersByReferralIdAsync(string referralId);
    }
}
