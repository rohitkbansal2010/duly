// -----------------------------------------------------------------------
// <copyright file="PatientConditionsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// --------------

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;
using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Api.Client;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="IPatientConditionsRepository"/>
    /// </summary>
    public class PatientConditionsRepository : IPatientConditionsRepository
    {
        private const string GetPatientConditionsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetPatientConditions]";
        private const string InsertPatientConditionsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertPatientConditions]";

        private readonly IDapperContext _dapperContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatientConditionsRepository(
        IDapperContext dapperContext,
        IHttpContextAccessor httpContextAccessor)
        {
            _dapperContext = dapperContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<long>> PostPatientConditionsAsync(Models.CarePlan.PatientConditions request, IDbTransaction transaction = null)
        {
            List<long> response = new List<long>();
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            foreach (var conditionId in request.AddConditionIds)
            {
                dynamic AddConditionsParameter = new
                {
                    PatientPlanId = request.PatientPlanId,
                    ConditionId = conditionId,
                    Deleted = false,
                    CreatedBy = userName
                };
                var dbResponse = await _dapperContext.ExecuteScalarAsync<long>(InsertPatientConditionsProcedureName, AddConditionsParameter, transaction);
                response.Add(dbResponse);
            }

            foreach (var conditionId in request.RemoveConditionIds)
            {
                dynamic RemoveConditionsParameter = new
                {
                    PatientPlanId = request.PatientPlanId,
                    ConditionId = conditionId,
                    Deleted = true,
                    UpdatedBy = userName
                };
                await _dapperContext.ExecuteScalarAsync<long>(InsertPatientConditionsProcedureName, RemoveConditionsParameter, transaction);
            }

            return response;
        }

        public async Task<IEnumerable<GetPatientConditionByPatientPlanIdModel>> GetConditionByPatientPlanId(long patientPlanId)
        {
            var parameters = new
            {
                PatientPlanId = patientPlanId
            };
            var result = await _dapperContext.QueryAsync<GetPatientConditionByPatientPlanIdModel>(GetPatientConditionsProcedureName, parameters);
            return result;
        }
    }
}