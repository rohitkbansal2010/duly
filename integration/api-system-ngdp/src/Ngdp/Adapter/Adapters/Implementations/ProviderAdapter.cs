// <copyright file="ProviderAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IProviderAdapter"/>
    /// </summary>
    internal class ProviderAdapter : IProviderAdapter
    {
        private const string FindProviderByLatLng = "GetProviderByLatLng";
        private const string FindProviderDetailsProcedureName = "[GetProviderLocation]";

        private readonly ICVDapperContext _dapperContext;

        public ProviderAdapter(ICVDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public Task<IEnumerable<ProviderLocation>> FindProviderByLatLngAsync(ProviderSearchCriteria searchCriteria)
        {
            if (searchCriteria == null)
            {
                throw new ArgumentNullException(nameof(searchCriteria));
            }

            var parameters = searchCriteria.ConvertToParameters();
            var data = _dapperContext.QueryAsync<ProviderLocation>(FindProviderByLatLng, parameters);

            return data;
        }

        public Task<IEnumerable<ProviderDetails>> FindProviderDetailsAsync(string providerIds)
        {
            var parameters = new
            {
                Id = providerIds
            };

            return _dapperContext.QueryAsync<ProviderDetails>(FindProviderDetailsProcedureName, parameters);
        }
    }
}
