// <copyright file="CustomTargetsAdapter.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="ICustomTargetsAdapter"/>
    /// </summary>
    internal class CustomTargetsAdapter : ICustomTargetsAdapter
    {
        //TODO: Move to setting files
        private const string InsertCustomTargetsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertCustomTargets]";

        private readonly ICVDapperContext _dapperContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomTargetsAdapter(ICVDapperContext dapperContext, IHttpContextAccessor httpContextAccessor)
        {
            _dapperContext = dapperContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<int> PostCustomTargetsAsync(CustomTargets request)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            dynamic CustomTargetsParameter = new
            {
                TargetName = request.TargetName,
                MinValue = request.MinValue,
                MaxValue = request.MaxValue,
                Range = request.Range,
                UnitOfMeasure = request.UnitOfMeasure,
                Description = request.Description,
                CreatedBy = userName
            };
            return _dapperContext.ExecuteScalarAsync<int>(InsertCustomTargetsProcedureName, CustomTargetsParameter);
        }
    }
}
