// <copyright file="PatientLifeGoalAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan;
using Duly.Ngdp.Adapter.Adapters.Models.CarePlan;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="IPatientLifeGoalAdapter"/>
    /// </summary>
    internal class PatientLifeGoalAdapter : IPatientLifeGoalAdapter
    {
        //TODO: Move to setting files
        private const string InsertLifeGoalProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertPatientLifeGoal]";

        private readonly ICVDapperContext _dapperContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatientLifeGoalAdapter(ICVDapperContext dapperContext, IHttpContextAccessor httpContextAccessor)
        {
            _dapperContext = dapperContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<int> PostPatientLifeGoalAsync(PatientLifeGoal request)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            dynamic PatientLifeGoalParameter = new
            {
                LifeGoalName = request.LifeGoalName,
                LifeGoalDescription = request.LifeGoalDescription,
                PatientPlanId = request.PatientPlanId,
                CreatedBy = userName
            };
            return _dapperContext.ExecuteScalarAsync<int>(InsertLifeGoalProcedureName, PatientLifeGoalParameter);
        }
    }
}
