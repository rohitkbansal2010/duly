// <copyright file="PatientTargetsRepositoryTests.cs" company="Duly Health and Care">
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
    public class PatientTargetsRepositoryTests
    {
        private const string GetPatientTargetProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetPatientTargets]";
        private const string PostPatientTargetProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspInsertPatientTargets]";
        private const string DeletePatientTargetProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspDeletePatientTarget]";
        private const string UpdatePatientTargetIsReviewedStatusProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspUpdatePatientTargetIsReviewedStatus]";
        private const string UpdatePatientTargetProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspUpdatePatientTargets]";

        private Mock<IDapperContext> _dapperMock;
        private Mock<IHttpContextAccessor> _iHttpContextAccessorMock;
        private PatientTargetsRepository _repositoryMock;
        private int _patientTargetId = 1; 

        [SetUp]
        public void SetUp()
        {
            _dapperMock = new Mock<IDapperContext>();
            _iHttpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _ = MockHelper.SetupHttpContext(_iHttpContextAccessorMock);
            _repositoryMock = new PatientTargetsRepository(_dapperMock.Object, _iHttpContextAccessorMock.Object);
        }

        [Test]
        public async Task GetPatientTargtesSuccessTest()
        {
            //Arrange
            var patientTargets = SetupGetPatientTargetsByPatientPlanIdDapperContext();

            //Act
            var result = await _repositoryMock.GetPatientTargetsByPatientPlanIdAsync(1);

            //Assert
            result.Should().BeEquivalentTo(patientTargets);
        }

        [Test]
        public async Task PostPatientTargetsByPatientPlanAsyncSuccessTest()
        {
            var response = SetupPostPatientTargetDapperContext();

            //Act
            PatientTargets patientPlan = new PatientTargets()
            {
                ConditionId = 1,
                TargetId = 1,
                MinValue = "TestMinValue",
                MaxValue = "TestMaxValue",
                MeasurementUnit = "TestMeasurementUnit",
                CreatedBy = "TestCreatedBy",
                UpdatedBy = "TestUpdatedBy",
                TargetValue = "1",
                BaseValue = "1",
                RecentValue = "1"
            };

            var result = await _repositoryMock.PostPatientTargetsAsync(patientPlan);

            //Assert
            result.Should().Be(response);
        }

        [Test]
        public async Task DeletePatientTargetAsyncSuccessTest()
        {
            var response = SetupDeletePatientTargetDapperContext();

            //Act
            var result = await _repositoryMock.DeletePatientTargetAsync(1);

            //Assert
            result.Should().Be(response);
        }

        [Test]
        public async Task UpdatePatientTargetReviewStatusAsyncSuccessTest()
        {
            UpdatePatientTargetReviewStatus updatePatientTargetReviewStatus = new UpdatePatientTargetReviewStatus()
            {
                IsReviewed = true
            };

            var response = SetupUpdatePatientTargetReviewStatusDapperContext();

            //Act
            var result = await _repositoryMock.UpdatePatientTargetReviewStatusAsync(updatePatientTargetReviewStatus, _patientTargetId);

            //Assert
            result.Should().Be(response);
        }

        [Test]
        public async Task UpdatePatientTargetsAsyncSuccessTest()
        {
            UpdatePatientTargets updatePatientTargets = new UpdatePatientTargets()
            {
                ConditionIds = new List<long>() { 1, 2 },
                TargetId = 1,
                MinValue = "TestMinValue",
                MaxValue = "TestMaxValue",
                MeasurementUnit = "TestMeasurementUnit",
                TargetValue = "1",
                BaseValue = "1",
                RecentValue = "1"
            };

            var response = SetupUpdatePatientTargetProcedureNameDapperContext();

            //Act
            var result = await _repositoryMock.UpdatePatientTargetsAsync(updatePatientTargets, _patientTargetId);

            //Assert
            result.Should().Be(response);
        }

        private IEnumerable<GetPatientTargets> SetupGetPatientTargetsByPatientPlanIdDapperContext()
        {
            IEnumerable<GetPatientTargets> conditionTargets = new GetPatientTargets[]
            {
                new()
            };

            _dapperMock
                .Setup(context =>
                context.QueryAsync<GetPatientTargets>(GetPatientTargetProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(conditionTargets);

            return conditionTargets;
        }

        private long SetupPostPatientTargetDapperContext()
        {
            _dapperMock
               .Setup(context =>
                context.ExecuteScalarAsync<long>(PostPatientTargetProcedureName, It.IsAny<object>(), default, default))
               .ReturnsAsync(1);

            return 1;
        }

        private int SetupDeletePatientTargetDapperContext()
        {
            _dapperMock
               .Setup(context =>
                context.ExecuteScalarAsync<int>(DeletePatientTargetProcedureName, It.IsAny<object>(), default, default))
               .ReturnsAsync(1);

            return 1;
        }

        private long SetupUpdatePatientTargetReviewStatusDapperContext()
        {
            _dapperMock
               .Setup(context =>
                context.ExecuteScalarAsync<long>(UpdatePatientTargetIsReviewedStatusProcedureName, It.IsAny<object>(), default, default))
               .ReturnsAsync(1);

            return 1;
        }

        private long SetupUpdatePatientTargetProcedureNameDapperContext()
        {
            _dapperMock
               .Setup(context =>
                context.ExecuteScalarAsync<long>(UpdatePatientTargetProcedureName, It.IsAny<object>(), default, default))
               .ReturnsAsync(1);

            return 1;
        }
    }
}