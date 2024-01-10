// <copyright file="PatientLifeGoalRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Dapper;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;
using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Api.Client;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="IPatientLifeGoalRepository"/>
    /// </summary>
    public class PatientLifeGoalRepository : IPatientLifeGoalRepository
    {
        private const string GetPatientLifeGoalsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetPatientLifeGoals]";

        private const string InsertLifeGoalsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertPatientLifeGoal]";

        private const string DeletePatientLifeGoalProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspDeletePatientLifeGoal]";

        private const string InsertPatienLifeTargetMappingProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertPatientLifeGoalTargetMapping]";

        private const string GetPatienLifeTargetMappingProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetPatientLifeGoalTargetMapping]";

        private const string GetPatientLifeGoalAndActionTrackingProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetPatientLifeGoalAndActionTracking]";

        private readonly IDapperContext _dapperContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatientLifeGoalRepository(
            IDapperContext dapperContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _dapperContext = dapperContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PostOrUpdatePatientLifeGoalResponse> PostOrUpdateLifeGoalAsync(Models.CarePlan.PostRequestForLifeGoals request, IDbTransaction transaction = null)
        {
            var response = new PostOrUpdatePatientLifeGoalResponse();

            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            // Post or Update new LifeGoals
            if (request.PatientPlanId > 0)
            {
                List<Models.CarePlan.PatientLifeGoal> insertPatientLifeGoalResponse = new List<Models.CarePlan.PatientLifeGoal>();

                foreach (var item in request.PatientLifeGoal)
                {
                    dynamic PatientLifeGoalParameter = new
                    {
                        PatientPlanId = request.PatientPlanId,
                        PatientLifeGoalId = item.PatientLifeGoalId,
                        LifeGoalName = item.LifeGoalName,
                        LifeGoalDescription = item.LifeGoalDescription,
                        CategoryName = item.CategoryName,
                        Priority = item.Priority,
                        CreatedBy = (item.PatientLifeGoalId == 0) ? userName : null,
                        UpdatedBy = (item.PatientLifeGoalId > 0) ? userName : null
                    };
                    var result = await _dapperContext.QueryFirstOrDefaultAsync<Models.CarePlan.PatientLifeGoal>(InsertLifeGoalsProcedureName, PatientLifeGoalParameter, transaction);
                    insertPatientLifeGoalResponse.Add(result);
                }

                response.PatientLifeGoals = insertPatientLifeGoalResponse;
            }

            // Delete Life Goals
            if (request.DeletedLifeGoalIds != null)
            {
                var commaSeparatedDeletedLifeGoalIds = string.Join(",", request.DeletedLifeGoalIds);

                dynamic DeleteLifeGoalParameter = new
                {
                    DeletedLifeGoalIds = commaSeparatedDeletedLifeGoalIds,
                    UpdatedBy = userName
                };
                var deleteResponse = await _dapperContext.ExecuteScalarAsync<string>(InsertLifeGoalsProcedureName, DeleteLifeGoalParameter, transaction);
                response.DeletedLifeGoalIds = deleteResponse;
            }

            return response;
        }

        public async Task<long> DeletePatientLifeGoalAsync(long patientLifeGoalId)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            dynamic DeleteLifeGoalParameter = new
            {
                PatientLifeGoalId = patientLifeGoalId,
                UpdatedBy = userName
            };
            long response = await _dapperContext.ExecuteScalarAsync<int>(DeletePatientLifeGoalProcedureName, DeleteLifeGoalParameter);

            return response;
        }

        public async Task<IEnumerable<GetPatientLifeGoalByPatientPlanIdModel>> GetPatientLifeGoalByPatientPlanIdAsync(long id)
        {
            var parameters = new
            {
                PatientPlanId = id
            };
            var result = await _dapperContext.QueryAsync<GetPatientLifeGoalByPatientPlanIdModel>(GetPatientLifeGoalsProcedureName, parameters);
            return result;
        }

        public async Task<long> PostPatientLifeGoalTargetMappingAsync(long patientTargetId, IEnumerable<long> patientLifeGoalIds, IDbTransaction transaction = null)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            var commaSeparatedLifeGoalIds = (patientLifeGoalIds.Count() > 0) ? string.Join(",", patientLifeGoalIds) : null ;

            var Parameters = new
            {
                PatientTargetId = patientTargetId,
                PatientLifeGoalIds = commaSeparatedLifeGoalIds,
                CreatedBy = userName
            };

            var response = await _dapperContext.QueryFirstOrDefaultAsync<long>(InsertPatienLifeTargetMappingProcedureName, Parameters, transaction);
            return response;
        }

        public async Task<IEnumerable<GetPatientLifeGoalTargetMapping>> GetPatientLifeGoalTargrtMappingByPatientIdAsync(long patientTargetId)
        {
            var parameters = new
            {
                PatientTargetId = patientTargetId
            };
            var result = await _dapperContext.QueryAsync<GetPatientLifeGoalTargetMapping>(GetPatienLifeTargetMappingProcedureName, parameters);
            return result;
        }

        public async Task<IEnumerable<PatientLifeGoalAndActionTracking>> GetPatientLifeGoalAndActionTrackingAsync(long patientPlanId)
        {
            var parameters = new
            {
                PatientPlanId = patientPlanId
            };
            var result = await _dapperContext.QueryAsync<PatientLifeGoalAndActionTracking>(GetPatientLifeGoalAndActionTrackingProcedureName, parameters);
            return result;
        }
    }
}
