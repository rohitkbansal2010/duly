// <copyright file="AuditAccountIdentityService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Audit;
using Duly.Common.Infrastructure.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Web;
using System;
using System.Linq;
using System.Security.Claims;

namespace Duly.Clinic.Fhir.Adapter.Context
{
    public class AuditAccountIdentityService : IAuditAccountIdentityService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly AuditUser _auditUser;
        private readonly string _appId;

        public AuditAccountIdentityService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _auditUser = new AuditUser
            {
                Upn = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.Upn),
                FirstName = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.GivenName),
                LastName = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.Surname),
                Roles = _accessor.HttpContext.User.HasClaim(claim => claim.Type == ClaimConstants.Role) ?
                    _accessor.HttpContext.User.FindAll(ClaimConstants.Role).Select(claim => claim.Value).ToArray()
                    : null
            };
            _appId = _accessor.HttpContext.User.FindFirstValue("appid");
        }

        public AuditUser GetUser() => _auditUser;

        public string GetAppId() => _appId;

        public Guid GetXCorrelationId()
        {
            if (_accessor.HttpContext.Response.Headers.TryGetValue(ParameterNames.XCorrelationIdHeader, out var headerValue)
                && Guid.TryParse(headerValue.ToString(), out var correlationId))
            {
                return correlationId;
            }

            return Guid.Empty;
        }
    }
}