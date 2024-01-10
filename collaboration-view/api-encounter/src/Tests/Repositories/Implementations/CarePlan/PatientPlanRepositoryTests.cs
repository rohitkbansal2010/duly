// <copyright file="PatientPlanRepositoryTests.cs" company="Duly Health and Care">
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
    public class PatientPlanRepositoryTests
    {
        private const string GetPatientPlanDetailsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetCarePlanForPatient]";
        private const string InsertPatientPlanProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertPatientPlan]";
        private const string UpdatePatientPlanStatusByIdProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspUpdatePatientPlanStatusById]";
        private Mock<IDapperContext> _dapperMock;
        private Mock<IHttpContextAccessor> _iHttpContextAccessorMock;
        private long _id = 1;

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
            var repositoryMock = new PatientPlanRepository(_dapperMock.Object, _iHttpContextAccessorMock.Object);
            var response = SetupGetPatientPlanIdByPatientIdDapperContext();

            //Act
            var result = await repositoryMock.GetPatientPlanIdByPatientIdAsync("t");

            //Assert
            result.Should().BeEquivalentTo(response);
        }

        [Test]
        public async Task PostPatientPlanAsyncSuccessTest()
        {
            _ = SetupHttpContext();

            var repositoryMock = new PatientPlanRepository(_dapperMock.Object, _iHttpContextAccessorMock.Object);
            var response = SetupPostPatientPlanDapperContext();

            //Act
            PatientPlan patientPlan = new PatientPlan()
            {
                AppointmentId = "TestAppointment",
                FlourishingStatement = "TestFlourishingStatement",
                PatientId = "TestPatientId",
                ProviderId = "TestProvidedId"
            };

            var result = await repositoryMock.PostPatientPlanAsync(patientPlan);

            //Assert
            result.Should().Be(response);
        }

        [Test]
        public async Task UpdatePatientPlanStatusByIdAsyncSuccessTest()
        {
            _ = SetupHttpContext();

            var repositoryMock = new PatientPlanRepository(_dapperMock.Object, _iHttpContextAccessorMock.Object);
            var response = SetupUpdatePatientPlanStatusByIdDapperContext();

            //Act
            var result = await repositoryMock.UpdatePatientPlanStatusByIdAsync(_id);

            //Assert
            result.Should().Be(response);
        }

        [Test]
        public async Task UpdateFlourishStatementAsyncSuccessTest()
        {
            var repositoryMock = new PatientPlanRepository(_dapperMock.Object, _iHttpContextAccessorMock.Object);
            var response = SetupUpdateFlourishStatementDapperContext();

            //Act
            var request = new Contracts.CarePlan.UpdateFlourishStatementRequest
            {
                FlourishStatement = "TestFlourishStatement",
                PatientPlanId = 99
            };

            var result = await repositoryMock.UpdateFlourishStatementAsync(request);

            //Assert
            result.Should().Be(response);
        }

        private GenericPrincipal SetupHttpContext()
        {
            var identity = new GenericIdentity("testUser@xyz123.com");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "1"));
            var principal = new GenericPrincipal(identity, new[] { "user" });
            _iHttpContextAccessorMock.Setup(s => s.HttpContext.User).Returns(principal);

            return principal;
        }

        private GetPatientPlanByPatientIdModel SetupGetPatientPlanIdByPatientIdDapperContext()
        {
            var response = new GetPatientPlanByPatientIdModel()
            {
                PlanName = "TestPlanName",
                PatientPlanId = 99,
                FlourishStatement = "TestFlourishStatement",
                IsCompleted = true
            };

            _dapperMock
                .Setup(context =>
                 context.QueryFirstOrDefaultAsync<GetPatientPlanByPatientIdModel>(GetPatientPlanDetailsProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(response);

            return response;
        }

        private int SetupPostPatientPlanDapperContext()
        {
            _dapperMock
               .Setup(context =>
                context.ExecuteScalarAsync<int>(InsertPatientPlanProcedureName, It.IsAny<object>(), default, default))
               .ReturnsAsync(1);

            return 1;
        }

        private bool SetupUpdatePatientPlanStatusByIdDapperContext()
        {
            _dapperMock
               .Setup(context =>
                context.ExecuteScalarAsync<bool>(UpdatePatientPlanStatusByIdProcedureName, It.IsAny<object>(), default, default))
               .ReturnsAsync(true);

            return true;
        }

        private long SetupUpdateFlourishStatementDapperContext()
        {
            _dapperMock
               .Setup(context =>
                context.ExecuteScalarAsync<long>(InsertPatientPlanProcedureName, It.IsAny<object>(), default, default))
               .ReturnsAsync(1);

            return 1;
        }
    }
}