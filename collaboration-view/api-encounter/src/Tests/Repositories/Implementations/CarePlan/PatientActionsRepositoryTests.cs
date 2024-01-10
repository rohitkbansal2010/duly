// <copyright file="PatientActionsRepositoryTests.cs" company="Duly Health and Care">
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
    public class PatientActionsRepositoryTests
    {
        private const string GetPatientActionsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetPatientActions]";
        private const string InsertPatientActionsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertPatientActions]";
        private const string UpdatePatientProgressProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspUpdatePatientProgress]";

        private Mock<IDapperContext> _dapperMock;
        private Mock<IHttpContextAccessor> _iHttpContextAccessorMock;

        [SetUp]
        public void SetUp()
        {
            _dapperMock = new Mock<IDapperContext>();
            _iHttpContextAccessorMock = new Mock<IHttpContextAccessor>();
        }

        [Test]
        public async Task GetPatientActionsByPatientTargetIdSuccessTest()
        {
            //Arrange
            var repositoryMock = new PatientActionsRepository(_dapperMock.Object, _iHttpContextAccessorMock.Object);
            var conditionTargets = SetupDapperContext();

            //Act
            var result = await repositoryMock.GetPatientActionsByPatientTargetIdAsync(1);

            //Assert
            result.Should().BeEquivalentTo(conditionTargets);
        }

        public async Task PostPatientActionsAsyncSuccessTest()
        {
            _ = SetupHttpContext();

            IEnumerable<Api.Repositories.Models.CarePlan.PatientActions> patientActions = new Api.Repositories.Models.CarePlan.PatientActions[]
            {
               new()
               {
                   PatientTargetId = 2,
                   ActionId = 1,
                   PatientActionId = 9,
                   CustomActionId = 8,
                   Deleted = false
               }
            };

            //Arrange
            var repositoryMock = new PatientActionsRepository(_dapperMock.Object, _iHttpContextAccessorMock.Object);
            var response = SetupPostPatientActionsDapperContext();

            //Act
            var result = await repositoryMock.PostPatientActionsAsync(patientActions);

            //Assert
            result.Should().Be(response);
        }

        public async Task UpdateActionProgressAsyncSuccessTest()
        {
            _ = SetupHttpContext();

            long id = 4;
            var patientActionUpdate = new UpdateActionProgress()
            {
              Notes = "Test notes",
              Progress = 8
            };

            //Arrange
            var repositoryMock = new PatientActionsRepository(_dapperMock.Object, _iHttpContextAccessorMock.Object);
            var response = SetupUpdateActionProgressDapperContext();

            //Act
            var result = await repositoryMock.UpdateActionProgressAsync(patientActionUpdate, id);

            //Assert
            result.Should().Be(response);
        }

        private IEnumerable<GetPatientActions> SetupDapperContext()
        {
            IEnumerable<GetPatientActions> conditionTargets = new GetPatientActions[]
            {
                new()
            };

            _dapperMock
                .Setup(context =>
                context.QueryAsync<GetPatientActions>(GetPatientActionsProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(conditionTargets);

            return conditionTargets;
        }

        private long SetupPostPatientActionsDapperContext()
        {
            _dapperMock
                .Setup(context =>
                context.ExecuteScalarAsync<long>(InsertPatientActionsProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(2);

            return 2;
        }

        private long SetupUpdateActionProgressDapperContext()
        {
            _dapperMock
                .Setup(context =>
                context.ExecuteScalarAsync<long>(UpdatePatientProgressProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(2);

            return 2;
        }

        private GenericPrincipal SetupHttpContext()
        {
            var identity = new GenericIdentity("testUser@xyz123.com");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "1"));
            var principal = new GenericPrincipal(identity, new[] { "user" });
            _iHttpContextAccessorMock.Setup(s => s.HttpContext.User).Returns(principal);

            return principal;
        }
    }
}
