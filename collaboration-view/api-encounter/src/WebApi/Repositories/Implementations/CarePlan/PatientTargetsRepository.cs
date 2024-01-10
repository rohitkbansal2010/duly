// <copyright file="PatientTargetsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;
using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Api.Client;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="IPatientTargetsRepository"/>
    /// </summary>
    public class PatientTargetsRepository : IPatientTargetsRepository
    {
        private const string GetPatientTargetProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetPatientTargets]";

        private const string PostPatientTargetProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertPatientTargets]";

        private const string DeletePatientTargetProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspDeletePatientTarget]";

        private const string UpdatePatientTargetIsReviewedStatusProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspUpdatePatientTargetIsReviewedStatus]";

        private const string UpdatePatientTargetProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspUpdatePatientTargets]";

        private readonly IDapperContext _dapperContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatientTargetsRepository(
            IDapperContext dapperContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _dapperContext = dapperContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Models.CarePlan.GetPatientTargets>> GetPatientTargetsByPatientPlanIdAsync(long patientPlanId)
        {
            var parameters = new
            {
                PatientPlanId = patientPlanId
            };
            var result = await _dapperContext.QueryAsync<Models.CarePlan.GetPatientTargets>(GetPatientTargetProcedureName, parameters);
            return result;
        }

        public async Task<long> PostPatientTargetsAsync(Models.CarePlan.PatientTargets request)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var createdBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            var parameters = new
            {
                PatientPlanId = request.PatientPlanId,
                ConditionId = request.ConditionId,
                TargetId = request.TargetId,
                MinValue = request.MinValue,
                MaxValue = request.MaxValue,
                MeasurementUnit = request.MeasurementUnit,
                TargetValue = request.TargetValue,
                BaseValue = request.BaseValue,
                RecentValue = request.RecentValue,
                CreatedBy = createdBy
            };
            var patientTargetId = await _dapperContext.ExecuteScalarAsync<long>(PostPatientTargetProcedureName, parameters);

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

        public async Task<long> UpdatePatientTargetReviewStatusAsync(UpdatePatientTargetReviewStatus request, long patientTargetId)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var UpdatedBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            long response = 0;
            var parameters = new
            {
                PatientTargetId = patientTargetId,
                IsReviewed = request.IsReviewed,
                UpdatedBy = UpdatedBy
            };
            response = await _dapperContext.ExecuteScalarAsync<long>(UpdatePatientTargetIsReviewedStatusProcedureName, parameters);

            return response;
        }

        public async Task<long> UpdatePatientTargetsAsync(UpdatePatientTargets request, long patientTargetId)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var UpdatedBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            var parameters = new
            {
                PatientTargetId = patientTargetId,
                PatientPlanId = request.PatientPlanId,
                ConditionIds = request.ConditionIds.Any() ? string.Join(',', request.ConditionIds) : null,
                TargetId = request.TargetId,
                MinValue = request.MinValue,
                MaxValue = request.MaxValue,
                MeasurementUnit = request.MeasurementUnit,
                TargetValue = request.TargetValue,
                BaseValue = request.BaseValue,
                RecentValue = request.RecentValue,
                UpdatedBy
            };
            long response = await _dapperContext.ExecuteScalarAsync<long>(UpdatePatientTargetProcedureName, parameters);

            return response;
        }
    }
}