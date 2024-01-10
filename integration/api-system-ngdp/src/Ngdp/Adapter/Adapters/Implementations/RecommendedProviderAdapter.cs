// <copyright file="RecommendedProviderAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Implementations
{
    public class RecommendedProviderAdapter : IRecommendedProviderAdapter
    {
        private const string FindRecommendedProvidersByReferralIdProcedureName = Constants.SchemaName + Constants.NameSeparator + "[uspReferralRecommendedProvidersSelect]";
        private readonly IDapperContext _dapperContext;

        public RecommendedProviderAdapter(IDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public Task<IEnumerable<RecommendedProvider>> FindRecommendedProvidersByReferralIdAsync(string referralId)
        {
            dynamic parameters = new
            {
                ReferralId = referralId
            };

            return _dapperContext.QueryAsync<RecommendedProvider>(FindRecommendedProvidersByReferralIdProcedureName, parameters);
        }

        public Task<RecommendedProvider> FindFirstRecommendedProviderByReferralIdAsync(string referralId)
        {
            dynamic parameters = new
            {
                ReferralId = referralId
            };

            return _dapperContext.QueryFirstOrDefaultAsync<RecommendedProvider>(FindRecommendedProvidersByReferralIdProcedureName, parameters);
        }
    }
}
