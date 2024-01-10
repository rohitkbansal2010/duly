// <copyright file="CustomActionsAdapter.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="ICustomActionsAdapter"/>
    /// </summary>
    internal class CustomActionsAdapter : ICustomActionsAdapter
    {
        //TODO: Move to setting files
        private const string InsertCustomActionsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertCustomActions]";

        private readonly ICVDapperContext _dapperContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomActionsAdapter(ICVDapperContext dapperContext, IHttpContextAccessor httpContextAccessor)
        {
            _dapperContext = dapperContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<int> PostCustomTargetsAsync(CustomActions request)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            dynamic CustomTargetsParameter = new
            {
                ActionName = request.ActionName,
                Description = request.Description,
                CreatedBy = userName
            };
            return _dapperContext.ExecuteScalarAsync<int>(InsertCustomActionsProcedureName, CustomTargetsParameter);
        }
    }
}
