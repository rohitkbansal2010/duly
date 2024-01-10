// <copyright file="PatientConditionsAdapter.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="IPatientConditionsAdapter"/>
    /// </summary>
    internal class PatientConditionsAdapter : IPatientConditionsAdapter
    {
        //TODO: Move to setting files
        private const string GetPatientConditionsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetPatientConditions]";
        private const string InsertPatientConditionsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertPatientConditions]";

        private readonly ICVDapperContext _dapperContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PatientConditionsAdapter(ICVDapperContext dapperContext, IHttpContextAccessor httpContextAccessor)
        {
            _dapperContext = dapperContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<GetPatientConditions>> GetPatientConditionsAsync(long patientPlanId)
        {
            var parameters = new
            {
                PatientPlanId = patientPlanId
            };
            var result = await _dapperContext.QueryAsync<GetPatientConditions>(GetPatientConditionsProcedureName, parameters);
            return result;
        }

        public async Task<long> PostPatientConditionsAsync(PatientConditions request, IDbTransaction transaction = null)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            long PatientPlanId = 0;
            foreach (long Id in request.ConditionId)
            {
                dynamic PatientPlanParameter = new
                {
                    PatientPlanId = request.PatientPlanId,
                    ConditionId = Id,
                    CreatedBy = userName
                };
                PatientPlanId = await _dapperContext.ExecuteScalarAsync<long>(InsertPatientConditionsProcedureName, PatientPlanParameter, transaction);
            }

            return PatientPlanId;
        }
    }
}
