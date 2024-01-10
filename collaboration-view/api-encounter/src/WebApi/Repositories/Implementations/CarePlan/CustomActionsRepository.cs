// -----------------------------------------------------------------------
// <copyright file="CustomActionsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// --------------

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
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
    /// <inheritdoc cref="ICustomActionsRepository"/>
    /// </summary>
    public class CustomActionsRepository : ICustomActionsRepository
    {
        private const string InsertCustomActionsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertCustomActions]";

        private readonly IDapperContext _dapperContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomActionsRepository(
        IDapperContext dapperContext,
        IHttpContextAccessor httpContextAccessor)
        {
            _dapperContext = dapperContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<long> PostCustomActionsAsync(Models.CarePlan.CustomActions request)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            dynamic CustomTargetsParameter = new
            {
                PatientTargetId = request.PatientTargetId,
                ActionName = request.ActionName,
                Description = request.Description,
                IsSelected = request.IsSelected,
                CreatedBy = userName
            };
            var response = _dapperContext.ExecuteScalarAsync<long>(InsertCustomActionsProcedureName, CustomTargetsParameter);
            return response;
        }
    }
}