// <copyright file="PatientPlanAdapter.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="IPatientPlanAdapter"/>
    /// </summary>
    internal class PatientPlanAdapter : IPatientPlanAdapter
    {
        //TODO: Move to setting files
        private const string GetPatientPlanDetailsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetCarePlanForPatient]";
        private const string InsertPatientPlanProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertPatientPlan]";
        private const string UpdatePatientPlanStatusByIdProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspUpdatePatientPlanStatusById]";

        private readonly ICVDapperContext _dapperContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatientPlanAdapter(ICVDapperContext dapperContext, IHttpContextAccessor httpContextAccessor)
        {
            _dapperContext = dapperContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<GetPatientPlan>> GetPatientPlanAsync(string patientId)
        {
            var parameters = new
            {
                PatientId = patientId
            };
            var result = await _dapperContext.QueryAsync<GetPatientPlan>(GetPatientPlanDetailsProcedureName, parameters);
            return result;
        }

        public Task<int> PostPatientPlanAsync(PatientPlan request)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            dynamic PatientPlanParameter = new
            {
                PatientId = request.PatientId,
                ProviderId = request.ProviderId,
                CreatedBy = userName,
                FlourishStatement = request.FlourishingStatement,
                AppointmentId = request.AppointmentId
            };
            return _dapperContext.ExecuteScalarAsync<int>(InsertPatientPlanProcedureName, PatientPlanParameter);
        }

        public Task<bool> UpdatePatientPlanStatusByIdAsync(long id)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            dynamic PatientPlanParameter = new
            {
                PatientPlanId = id,
                UpdatedBy = userName
            };
            return _dapperContext.ExecuteScalarAsync<bool>(UpdatePatientPlanStatusByIdProcedureName, PatientPlanParameter);
        }
    }
}