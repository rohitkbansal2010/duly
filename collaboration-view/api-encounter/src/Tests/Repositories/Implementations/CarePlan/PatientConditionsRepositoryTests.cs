// <copyright file="PatientConditionsRepositoryTests.cs" company="Duly Health and Care">
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
    public class PatientConditionsRepositoryTests
    {
        private const string GetPatientConditionsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetPatientConditions]";
        private const string InsertPatientConditionsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertPatientConditions]";

        private Mock<IDapperContext> _dapperMock;
        private Mock<IHttpContextAccessor> _iHttpContextAccessorMock;
        private PatientConditionsRepository _repositoryMock;

        [SetUp]
        public void SetUp()
        {
            _dapperMock = new Mock<IDapperContext>();
            _iHttpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _ = MockHelper.SetupHttpContext(_iHttpContextAccessorMock);
            _repositoryMock = new PatientConditionsRepository(_dapperMock.Object, _iHttpContextAccessorMock.Object);
        }

        [Test]
        public async Task PostPatientConditionsAsyncSuccessTest()
        {
            //Arrange
            _ = SetupPostPatientConditionsDapperContext();

            //Act
            PatientConditions request = new PatientConditions()
            {
                AddConditionIds = new long[] { 1, 3 },
                PatientPlanId = 1,
                RemoveConditionIds = new long[] { 2, 4 }
            };
            var result = await _repositoryMock.PostPatientConditionsAsync(request);

            //Assert
            result.Should().NotBeNull();
        }

        [Test]
        public async Task GetConditionByPatientPlanIdSuccessTest()
        {
            //Arrange
            _ = SetupGetConditionByPatientPlanIdDapperContext();

            //Act
            var result = await _repositoryMock.GetConditionByPatientPlanId(1);

            //Assert
            result.Should().NotBeNull();
        }

        private long SetupPostPatientConditionsDapperContext()
        {
            _dapperMock
               .Setup(context =>
                context.ExecuteScalarAsync<long>(InsertPatientConditionsProcedureName, It.IsAny<object>(), default, default))
               .ReturnsAsync(1);

            return 1;
        }

        private IEnumerable<GetPatientConditionByPatientPlanIdModel> SetupGetConditionByPatientPlanIdDapperContext()
        {
            IEnumerable<GetPatientConditionByPatientPlanIdModel> response = new List<GetPatientConditionByPatientPlanIdModel>()
            {
                new GetPatientConditionByPatientPlanIdModel()
                {
                     ConditionDisplayName = "TestConditionDisplayName",
                     PatientConditionId = 1,
                     ConditionId = 2,
                     ConditionShortName = "TestConditionShortName"
                }
            };

            _dapperMock
               .Setup(context =>
                context.QueryAsync<GetPatientConditionByPatientPlanIdModel>(GetPatientConditionsProcedureName, It.IsAny<object>(), default, default))
               .ReturnsAsync(response);

            return response;
        }
    }
}