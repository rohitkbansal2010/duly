// <copyright file="MockHelper.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using System.Security.Principal;

namespace Duly.CollaborationView.Encounter.Api.Tests.Repositories.Helpers
{
    public static class MockHelper
    {
        //private readonly IHttpContextAccessor _httpContextAccessor;
        public static GenericPrincipal SetupHttpContext(Mock<IHttpContextAccessor> iHttpContextAccessorMock)
        {
            var identity = new GenericIdentity("testUser@xyz123.com");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "1"));
            var principal = new GenericPrincipal(identity, new[] { "user" });
            iHttpContextAccessorMock.Setup(s => s.HttpContext.User).Returns(principal);

            return principal;
        }
    }
}
