// -----------------------------------------------------------------------
// <copyright file="PatientActionsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// --------------

using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan;
using Duly.Common.DataAccess.Contexts.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="IPatientActionsRepository"/>
    /// </summary>
    public class PatientActionsRepository : IPatientActionsRepository
    {
        private const string GetPatientActionsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetPatientActions]";
        private const string InsertPatientActionsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertPatientActions]";
        private const string UpdatePatientProgressProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspUpdatePatientProgress]";
        private const string GetPatientActionStatsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetPatientActionStats]";

        private readonly IDapperContext _dapperContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatientActionsRepository(
        IDapperContext dapperContext,
        IHttpContextAccessor httpContextAccessor)
        {
            _dapperContext = dapperContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Models.CarePlan.GetPatientActions>> GetPatientActionsByPatientTargetIdAsync(long patientTargetId)
        {
            var parameters = new
            {
                PatientTargetId = patientTargetId
            };
            var result = await _dapperContext.QueryAsync<Models.CarePlan.GetPatientActions>(GetPatientActionsProcedureName, parameters);
            return result;
        }

        public async Task<long> PostPatientActionsAsync(IEnumerable<Models.CarePlan.PatientActions> request, IDbTransaction transaction = null)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            long TargetId = 0;
            foreach (var item in request)
            {
                dynamic patientActionsParameters = new
                {
                    PatientActionId = item.PatientActionId,
                    PatientTargetId = item.PatientTargetId,
                    ActionId = item.ActionId,
                    CustomActionId = item.CustomActionId,
                    Deleted = item.Deleted,
                    CreatedBy = userName
                };
                TargetId = await _dapperContext.ExecuteScalarAsync<long>(InsertPatientActionsProcedureName, patientActionsParameters, transaction);
            }

            return TargetId;
        }

        public async Task<long> UpdateActionProgressAsync(Models.CarePlan.UpdateActionProgress request, long id)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            long patientActionId = 0;

            dynamic updateActionProgressParameters = new
                {
                    PatientActionId = id,
                    Progress = request.Progress,
                    Notes = request.Notes,
                    UpdatedBy = userName
                };
            patientActionId = await _dapperContext.ExecuteScalarAsync<long>(UpdatePatientProgressProcedureName, updateActionProgressParameters);

            return patientActionId;
        }

        public async Task<Models.CarePlan.GetPatientActionStats> GetPatientActionStatsAsync(long patientPlanId)
        {
            var parameters = new
            {
                PatientPlanId = patientPlanId
            };
            var response = await _dapperContext.QueryFirstOrDefaultAsync<Models.CarePlan.GetPatientActionStats>(GetPatientActionStatsProcedureName, parameters);

            return response;
        }
    }
}