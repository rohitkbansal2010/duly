// <copyright file="TargetActionsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan;
using Duly.Common.DataAccess.Contexts.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="ITargetActionsRepository"/>
    /// </summary>
    public class TargetActionsRepository : ITargetActionsRepository
    {
        private const string GetTargetActionsByTargetIdProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetTargetActionsByConditionIdAndTargetId]";
        private readonly IDapperContext _dapperContext;

        public TargetActionsRepository(
            IDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<Models.CarePlan.TargetActions>> GetTargetActionsByConditionIdAndTargetIdAsync(long conditionId, long targetId)
        {
            var parameters = new
            {
                ConditionId = conditionId,
                TargetId = targetId
            };

            var result = await _dapperContext.QueryAsync<Models.CarePlan.TargetActions>(GetTargetActionsByTargetIdProcedureName, parameters);
            return result;
        }
    }
}