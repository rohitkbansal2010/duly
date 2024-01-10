// <copyright file="PatientLifeGoalRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Implementations.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;
using Duly.CollaborationView.Encounter.Api.Tests.Repositories.Helpers;
using Duly.Common.DataAccess.Contexts.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Repositories.Implementations.CarePlan
{
    [TestFixture]
    public class PatientLifeGoalRepositoryTests
    {
        private const string GetPatientLifeGoalsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetPatientLifeGoals]";
        private const string InsertLifeGoalsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertPatientLifeGoal]";
        private const string DeletePatientLifeGoalProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspDeletePatientLifeGoal]";
        private const string InsertPatienLifeTargetMappingProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertPatientLifeGoalTargetMapping]";
        private const string GetPatienLifeTargetMappingProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetPatientLifeGoalTargetMapping]";

        private Mock<IDapperContext> _dapperMock;
        private Mock<IHttpContextAccessor> _iHttpContextAccessorMock;
        private PatientLifeGoalRepository _repositoryMock;

        [SetUp]
        public void SetUp()
        {
            _dapperMock = new Mock<IDapperContext>();
            _iHttpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _ = MockHelper.SetupHttpContext(_iHttpContextAccessorMock);
            _repositoryMock = new PatientLifeGoalRepository(_dapperMock.Object, _iHttpContextAccessorMock.Object);
        }

        [Test]
        public async Task PostOrUpdateLifeGoalAsyncSuccessTest()
        {
            //Arrange
            var response = SetupPostOrUpdateLifeGoalDapperContext();

            //Act
            PostRequestForLifeGoals request = new PostRequestForLifeGoals()
            {
                PatientLifeGoal = new List<PatientLifeGoal>()
                {
                   new PatientLifeGoal
                   {
                       CategoryName = "TestCategoryName",
                       LifeGoalDescription = "TestLifeGoalDescription",
                       LifeGoalName = "TestLifeGoalName",
                       PatientLifeGoalId = 99
                   }
                },
                DeletedLifeGoalIds = new List<long>() { 1, 2 },
                PatientPlanId = 1
            };
            var result = await _repositoryMock.PostOrUpdateLifeGoalAsync(request);

            //Assert
            result.Should().NotBeNull();
        }

        [Test]
        public async Task DeletePatientLifeGoalAsyncSuccessTest()
        {
            //Arrange
            _ = SetupDeletePatientLifeGoalDapperContext();

            //Act
            long patientLifeGoalId = 99;
            var result = await _repositoryMock.DeletePatientLifeGoalAsync(patientLifeGoalId);

            //Assert
            result.Should().Be(1);
        }

        [Test]
        public async Task GetPatientLifeGoalByPatientPlanIdAsyncSuccessTest()
        {
            //Arrange
            var response = SetupGetPatientLifeGoalByPatientPlanIdDapperContext();

            //Act
            long id = 99;
            var result = await _repositoryMock.GetPatientLifeGoalByPatientPlanIdAsync(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<GetPatientLifeGoalByPatientPlanIdModel>>();
        }

        [Test]
        public async Task PostPatientLifeGoalTargetMappingAsyncSuccessTest()
        {
            //Arrange
            var response = SetupPostPatientLifeGoalTargetMappingDapperContext();

            //Act
            long PatientTargetId = 98;
            List<long> PatientLifeGoalId = new List<long>() { 99 };

            var result = await _repositoryMock.PostPatientLifeGoalTargetMappingAsync(PatientTargetId, PatientLifeGoalId, null);

            //Assert
            result.Should().Be(response);
        }

        [Test]
        public async Task GetPatientLifeGoalTargrtMappingByPatientIdAsyncSuccessTest()
        {
            //Arrange
            var response = SetupGetPatientLifeGoalTargrtMappingByPatientIdDapperContext();

            //Act
            long patientTargetId = 99;
            var result = await _repositoryMock.GetPatientLifeGoalTargrtMappingByPatientIdAsync(patientTargetId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<GetPatientLifeGoalTargetMapping>>();
        }

        private PatientLifeGoal SetupPostOrUpdateLifeGoalDapperContext()
        {
            PatientLifeGoal response = new PatientLifeGoal()
            {
                PatientLifeGoalId = 99,
                CategoryName = "TestCategoryName",
                LifeGoalDescription = "TestLifeGoalDescription",
                LifeGoalName = "TestLifeGoalName"
            };

            _dapperMock
               .Setup(context =>
                context.QueryFirstOrDefaultAsync<PatientLifeGoal>(InsertLifeGoalsProcedureName, It.IsAny<object>(), default, default))
               .ReturnsAsync(response);

            return response;
        }

        private int SetupDeletePatientLifeGoalDapperContext()
        {
            _dapperMock
               .Setup(context =>
                context.ExecuteScalarAsync<int>(DeletePatientLifeGoalProcedureName, It.IsAny<object>(), default, default))
               .ReturnsAsync(1);

            return 1;
        }

        private IEnumerable<GetPatientLifeGoalByPatientPlanIdModel> SetupGetPatientLifeGoalByPatientPlanIdDapperContext()
        {
            var res = new List<GetPatientLifeGoalByPatientPlanIdModel>()
            {
                new GetPatientLifeGoalByPatientPlanIdModel()
                {
                    PatientLifeGoalId = 99,
                    CategoryName = "TestCategoryName",
                    LifeGoalDescription = "TestLifeGoalDescription",
                    LifeGoalName = "TestLifeGoalName"
                }
            };

            _dapperMock
               .Setup(context =>
                context.QueryAsync<GetPatientLifeGoalByPatientPlanIdModel>(GetPatientLifeGoalsProcedureName, It.IsAny<object>(), default, default))
               .ReturnsAsync(res);

            return res;
        }

        private long SetupPostPatientLifeGoalTargetMappingDapperContext()
        {
            _dapperMock
               .Setup(context =>
                context.QueryFirstOrDefaultAsync<long>(InsertPatienLifeTargetMappingProcedureName, It.IsAny<object>(), default, default))
               .ReturnsAsync(1);

            return 1;
        }

        private IEnumerable<GetPatientLifeGoalTargetMapping> SetupGetPatientLifeGoalTargrtMappingByPatientIdDapperContext()
        {
            var res = new List<GetPatientLifeGoalTargetMapping>()
            {
                new GetPatientLifeGoalTargetMapping()
                {
                    PatientLifeGoalId = 99,
                    LifeGoalDescription = "TestLifeGoalDescription",
                    LifeGoalName = "TestLifeGoalName"
                }
            };

            _dapperMock
               .Setup(context =>
                context.QueryAsync<GetPatientLifeGoalTargetMapping>(GetPatienLifeTargetMappingProcedureName, It.IsAny<object>(), default, default))
               .ReturnsAsync(res);

            return res;
        }
    }
}