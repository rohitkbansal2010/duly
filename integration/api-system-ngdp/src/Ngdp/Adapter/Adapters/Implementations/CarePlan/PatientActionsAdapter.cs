// <copyright file="PatientActionsAdapter.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="IPatientActionsAdapter"/>
    /// </summary>
    internal class PatientActionsAdapter : IPatientActionsAdapter
    {
        //TODO: Move to setting files
        private const string GetPatientActionsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetPatientActions]";
        private const string InsertPatientActionsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertPatientActions]";

        private readonly ICVDapperContext _dapperContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PatientActionsAdapter(ICVDapperContext dapperContext, IHttpContextAccessor httpContextAccessor)
        {
            _dapperContext = dapperContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<GetPatientActions>> GetPatientActionsAsync(long patientTargetId)
        {
            var parameters = new
            {
                PatientTargetId = patientTargetId
            };
            var result = await _dapperContext.QueryAsync<GetPatientActions>(GetPatientActionsProcedureName, parameters);
            return result;
        }

        public async Task<long> PostPatientActionsAsync(IEnumerable<PatientActions> request, IDbTransaction transaction = null)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            long TargetId = 0;
            foreach (var item in request)
            {
                dynamic patientActionsParameters = new
                {
                    PatientTargetId = item.PatientTargetId,
                    ActionId = item.ActionId,
                    ActionType = item.ActionType,
                    CustomActionId = item.CustomActionId,
                    Deleted = item.Deleted,
                    CreatedBy = userName
                };
                TargetId = await _dapperContext.ExecuteScalarAsync<long>(InsertPatientActionsProcedureName, patientActionsParameters, transaction);
            }

            return TargetId;
        }
    }
}
