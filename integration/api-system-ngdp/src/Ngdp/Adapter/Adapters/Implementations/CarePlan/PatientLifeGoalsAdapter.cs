// <copyright file="PatientLifeGoalsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan;
using Duly.Ngdp.Adapter.Adapters.Models.CarePlan;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="IPatientLifeGoalsAdapter"/>
    /// </summary>
    internal class PatientLifeGoalsAdapter : IPatientLifeGoalsAdapter
    {
        private const string GetPatientLifeGoalsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetPatientLifeGoals]";

        private const string InsertLifeGoalsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertPatientLifeGoal]";

        private const string DeletePatientLifeGoalProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspDeletePatientLifeGoal]";


        private readonly ICVDapperContext _dapperContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatientLifeGoalsAdapter(ICVDapperContext dapperContext, IHttpContextAccessor httpContextAccessor)
        {
            _dapperContext = dapperContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<GetPatientLifeGoals>> GetPatientLifeGoalsAsync(long patientPlanId)
        {
            var parameters = new
            {
                PatientPlanId = patientPlanId
            };
            var result = await _dapperContext.QueryAsync<GetPatientLifeGoals>(GetPatientLifeGoalsProcedureName, parameters);
            return result;
        }

        public async Task<List<PatientLifeGoalResponse>> PostOrUpdateLifeGoalAsync(IEnumerable<PatientLifeGoals> request, IDbTransaction transaction = null)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            List<PatientLifeGoalResponse> patientLifeGoalResponse = new List<PatientLifeGoalResponse>();

            foreach (var item in request)
            {

                dynamic PatientLifeGoalParameter = new
                {
                    PatientLifeGoalId = item.PatientLifeGoalId,
                    LifeGoalName = item.LifeGoalName,
                    LifeGoalDescription = item.LifeGoalDescription,
                    PatientPlanId = item.PatientPlanId,
                    CreatedBy = userName
                };
                var result = await _dapperContext.QueryFirstOrDefaultAsync<PatientLifeGoalResponse>(InsertLifeGoalsProcedureName, PatientLifeGoalParameter, transaction);
                patientLifeGoalResponse.Add(result);
            }

            return patientLifeGoalResponse;
        }

        public async Task<long> DeletePatientLifeGoalAsync(long id)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            dynamic DeleteLifeGoalParameter = new
            {
                PatientLifeGoalId = id,
                UpdatedBy = userName
            };
            long patientLifeGoalId = await _dapperContext.ExecuteScalarAsync<int>(DeletePatientLifeGoalProcedureName, DeleteLifeGoalParameter);
            return patientLifeGoalId;
        }
    }
}
