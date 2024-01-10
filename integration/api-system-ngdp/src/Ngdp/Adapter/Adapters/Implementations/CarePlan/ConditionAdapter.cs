﻿// <copyright file="ConditionAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan;
using Duly.Ngdp.Adapter.Adapters.Models.CarePlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="IConditionAdapter"/>
    /// </summary>
    internal class ConditionAdapter : IConditionAdapter
    {
        private const string GetAllActiveConditionsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetAllActiveConditions]";

        private readonly ICVDapperContext _dapperContext;

        public ConditionAdapter(ICVDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<Condition>> GetConditionsAsync()
        {
            var result = await _dapperContext.QueryAsync<Condition>(GetAllActiveConditionsProcedureName);
            return result;
        }
    }
}