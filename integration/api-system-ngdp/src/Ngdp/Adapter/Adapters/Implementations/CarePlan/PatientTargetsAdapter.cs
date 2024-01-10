// <copyright file="PatientTargetsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan;
using Duly.Ngdp.Adapter.Adapters.Models.CarePlan;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="ITargetActionsAdapter"/>
    /// </summary>
    internal class PatientTargetsAdapter : IPatientTargetsAdapter
    {
        private const string GetPatientTargetProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetPatientTargets]";

        private const string PostPatientTargetProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertPatientTargets]";

        private const string DeletePatientTargetProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspDeletePatientTarget]";

        private readonly ICVDapperContext _dapperContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatientTargetsAdapter(ICVDapperContext dapperContext, IHttpContextAccessor httpContextAccessor)
        {
            _dapperContext = dapperContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<GetPatientTargets>> GetPatientTargetsAsync(long patientPlanId)
        {
            var parameters = new
            {
                PatientPlanId = patientPlanId
            };
            var result = await _dapperContext.QueryAsync<GetPatientTargets>(GetPatientTargetProcedureName, parameters);
            return result;
        }

        public async Task<int> PostPatientTargetAsync(PatientTarget request)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var createdBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            var patientTargetId = 0;
            var parameters = new
            {
                PatientPlanId = request.PatientPlanId,
                ConditionId = request.ConditionId,
                TargetType = request.TargetType,
                TargetId = request.TargetId,
                CustomTargetId = request.CustomTargetId,
                MinValue = request.MinValue,
                MaxValue = request.MaxValue,
                MeasurementValue = request.MeasurementValue,
                MeasurementUnit = request.MeasurementUnit,
                CreatedBy = createdBy
            };
            patientTargetId = await _dapperContext.ExecuteScalarAsync<int>(PostPatientTargetProcedureName, parameters);

            return patientTargetId;

       }

        public async Task<int> DeletePatientTargetAsync(long id)
        {
                var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                dynamic DeleteTargetParameter = new
                {
                    PatientTargetId = id,
                    UpdatedBy = userName
                };
                int patientTargetId = await _dapperContext.ExecuteScalarAsync<int>(DeletePatientTargetProcedureName, DeleteTargetParameter);
                return patientTargetId;
        }
    }
}