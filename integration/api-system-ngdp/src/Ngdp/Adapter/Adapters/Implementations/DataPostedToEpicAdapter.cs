// <copyright file="DataPostedToEpicAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IDataPostedToEpicAdapter"/>
    /// </summary>
    internal class DataPostedToEpicAdapter : IDataPostedToEpicAdapter
    {
        private const string InsertDataPostedToEpicStoredProcedureName = "[UpdateDataPostedToEpic]";

        private readonly ICVDapperContext _dapperContext;

        public DataPostedToEpicAdapter(ICVDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public Task<int> PostDataPostedToEpicAsync(int id)
        {
            dynamic labDetailParameter = new
            {
                Id = id,
                DataPOstedToEpic = 1
            };
            _dapperContext.ExecuteScalarAsync(InsertDataPostedToEpicStoredProcedureName, labDetailParameter);
            return Task.FromResult(id);
        }
    }
}