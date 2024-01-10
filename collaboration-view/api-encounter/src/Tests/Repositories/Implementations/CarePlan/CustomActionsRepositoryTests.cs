// <copyright file="CustomActionsRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Implementations.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;
using Duly.Common.DataAccess.Contexts.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Repositories.Implementations.CarePlan
{
    [TestFixture]
    public class CustomActionsRepositoryTests
    {
        private const string InsertCustomActionsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertCustomActions]";
        private Mock<IDapperContext> _dapperMock;
        private Mock<IHttpContextAccessor> _httpContextAccessor;

        [SetUp]
        public void SetUp()
        {
            _dapperMock = new Mock<IDapperContext>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
        }

        [Test]
        public async Task PostCustomActionsSuccessTests()
        {
            _ = SetupHttpContext();

            var customActions = new Api.Repositories.Models.CarePlan.CustomActions
            {
                ActionName = "test action name",
                Description = "dummy description"
            };

            //Arrange
            var repositoryMock = new CustomActionsRepository(_dapperMock.Object, _httpContextAccessor.Object);
            var response = SetupDapperContext(customActions);

            //Act
            var result = await repositoryMock.PostCustomActionsAsync(customActions);

            //Assert
            result.Should().Be(response);
        }

        private GenericPrincipal SetupHttpContext()
        {
            var identity = new GenericIdentity("testUser@xyz123.com");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "1"));
            var principal = new GenericPrincipal(identity, new[] { "user" });
            _httpContextAccessor.Setup(s => s.HttpContext.User).Returns(principal);

            return principal;
        }

        private long SetupDapperContext(Api.Repositories.Models.CarePlan.CustomActions customActions)
        {
            long res = 1;

            _dapperMock
                .Setup(context =>
                context.ExecuteScalarAsync<long>(InsertCustomActionsProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(res);

            return res;
        }
    }
}