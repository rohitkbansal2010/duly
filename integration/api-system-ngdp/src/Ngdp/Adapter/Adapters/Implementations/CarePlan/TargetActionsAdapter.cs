// <copyright file="TargetActionsAdapter.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="ITargetActionsAdapter"/>
    /// </summary>
    internal class TargetActionsAdapter : ITargetActionsAdapter
    {
        private const string GetTargetActionsByTargetIdProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetTargetActionsByTargetId]";

        private readonly ICVDapperContext _dapperContext;

        public TargetActionsAdapter(ICVDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<TargetActions>> GetTargetActionsByTargetIdAsync(long targetId)
        {
            var parameters = new
            {
                TargetId = targetId
            };

            var result = await _dapperContext.QueryAsync<TargetActions>(GetTargetActionsByTargetIdProcedureName, parameters);
            return result;
        }
    }
}