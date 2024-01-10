// <copyright file="ConditionRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan;
using Duly.Common.DataAccess.Contexts.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="IConditionRepository"/>
    /// </summary>
    public class ConditionRepository : IConditionRepository
    {
        private const string GetAllActiveConditionsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetAllActiveConditions]";
        private readonly IDapperContext _dapperContext;

        public ConditionRepository(
            IDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<Models.CarePlan.Condition>> GetConditions()
        {
            var result = await _dapperContext.QueryAsync<Models.CarePlan.Condition>(GetAllActiveConditionsProcedureName);
            return result;
        }
    }
}