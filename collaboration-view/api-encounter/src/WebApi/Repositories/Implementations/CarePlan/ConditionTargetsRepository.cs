// <copyright file="ConditionTargetsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;
using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Api.Client;
using Microsoft.AspNetCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="IConditionTargetsRepository"/>
    /// </summary>
    public class ConditionTargetsRepository : IConditionTargetsRepository
    {
        private const string GetAllConditionTargetsByConditionIdsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetAllConditionTargetsByConditionIds]";

        private readonly IDapperContext _dapperContext;

        public ConditionTargetsRepository(
            IDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<Models.CarePlan.ConditionTargets>> GetConditionTargetsByConditionId(string conditionIds)
        {
            var parameters = new
            {
                ConditionIds = conditionIds
            };
            var result = await _dapperContext.QueryAsync<Models.CarePlan.ConditionTargets>(GetAllConditionTargetsByConditionIdsProcedureName, parameters);
            return result;
        }
    }
}