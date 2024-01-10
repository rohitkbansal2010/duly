// -----------------------------------------------------------------------
// <copyright file="PatientPlanRepository.cs" company="Duly Health and Care">
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
using System.Security.Claims;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="IPatientPlanRepository"/>
    /// </summary>
    public class PatientPlanRepository : IPatientPlanRepository
    {
        private const string GetPatientPlanDetailsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetCarePlanForPatient]";
        private const string InsertPatientPlanProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertPatientPlan]";
        private const string UpdatePatientPlanStatusByIdProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspUpdatePatientPlanStatusById]";
        private const string GetHealthPlanStatsByPatientPlanIdProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetHealthPlanStats]";
        private readonly IDapperContext _dapperContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatientPlanRepository(
        IDapperContext dapperContext,
        IHttpContextAccessor httpContextAccessor)
        {
            _dapperContext = dapperContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public DefaultHttpContext HttpContext { get; internal set; }

        public async Task<GetPatientPlanByPatientIdModel> GetPatientPlanIdByPatientIdAsync(string patientId)
        {
            var parameters = new
            {
                PatientId = patientId
            };
            var response = await _dapperContext.QueryFirstOrDefaultAsync<GetPatientPlanByPatientIdModel>(GetPatientPlanDetailsProcedureName, parameters);
            return response;
        }

        public async Task<int> PostPatientPlanAsync(Models.CarePlan.PatientPlan request)
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
            return await _dapperContext.ExecuteScalarAsync<int>(InsertPatientPlanProcedureName, PatientPlanParameter);
        }

        public async Task<bool> UpdatePatientPlanStatusByIdAsync(long id)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            dynamic PatientPlanParameter = new
            {
                PatientPlanId = id,
                UpdatedBy = userName
            };
            return await _dapperContext.ExecuteScalarAsync<bool>(UpdatePatientPlanStatusByIdProcedureName, PatientPlanParameter);
        }

        public async Task<long> UpdateFlourishStatementAsync(Contracts.CarePlan.UpdateFlourishStatementRequest request)
        {
            dynamic Parameter = new
            {
                PatientPlanId = request.PatientPlanId,
                FlourishStatement = request.FlourishStatement
            };
            var patientPlanId = await _dapperContext.ExecuteScalarAsync<long>(InsertPatientPlanProcedureName, Parameter);
            return patientPlanId;
        }

        public async Task<GetHealthPlanStats> GetHealthPlanStatsByPatientPlanIdAsync(long patientPlanId)
        {
            var Parameter = new
            {
                PatientPlanId = patientPlanId
            };

            var response = await _dapperContext.QuerySingleAsync<GetHealthPlanStats>(GetHealthPlanStatsByPatientPlanIdProcedureName, Parameter);
            return response;
        }
    }
}