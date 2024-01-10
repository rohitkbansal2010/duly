// <copyright file="ConditionTargetsAdapter.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="IConditionTargetsAdapter"/>
    /// </summary>
    internal class ConditionTargetsAdapter : IConditionTargetsAdapter
    {
        private const string GetAllConditionTargetsByConditionIdsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetAllConditionTargetsByConditionIds]";

        private readonly ICVDapperContext _dapperContext;

        public ConditionTargetsAdapter(ICVDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<ConditionTargets>> GetConditionTargetsByConditionIdAsync(string conditionIds)
        {
            var parameters = new
            {
                ConditionIds = conditionIds
            };

            var result = await _dapperContext.QueryAsync<ConditionTargets>(GetAllConditionTargetsByConditionIdsProcedureName, parameters);
            return result;
        }
    }
}