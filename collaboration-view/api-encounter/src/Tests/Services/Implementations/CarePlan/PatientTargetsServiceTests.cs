// <copyright file="PatientTargetsServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan;
using Duly.CollaborationView.Encounter.Api.Services.Implementations.CarePlan;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations.CarePlan
{
    [TestFixture]
    public class PatientTargetsServiceTests
    {
        private Mock<IPatientTargetsRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private long _patientTargetId = 1;

        private UpdatePatientTargetReviewStatus _updatePatientTargetReviewStatus = new UpdatePatientTargetReviewStatus()
        {
            IsReviewed = true
        };

        private PatientTargets _patientTarget = new PatientTargets()
        {
            ConditionId = 1,
            TargetId = 1,
            MinValue = "TestMinValue",
            MaxValue = "TestMaxValue",
            MeasurementUnit = "TestMeasurementValye",
            TargetValue = "1",
            BaseValue = "1",
            RecentValue = "1"
        };

        private UpdatePatientTargets _updatePatientTargets = new UpdatePatientTargets()
        {
            TargetId = 1,
            MinValue = "TestMinValue",
            MaxValue = "TestMaxValue",
            MeasurementUnit = "TestMeasurementValye",
            TargetValue = "1",
            BaseValue = "1",
            RecentValue = "1"
        };

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IPatientTargetsRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task GetPatientTargetsSuccessTests()
        {
            //Arrange
            IEnumerable<Models.GetPatientTargets> patientTargets = new List<Models.GetPatientTargets>()
            {
                 new()
                 {
                        TargetId = 1,
                        TargetName = "TestTargetName",
                        MinValue = "TestMinValue",
                        MaxValue = "TestMaxValue",
                        BaseValue = "1",
                        TargetValue = "1",
                        RecentValue = "1",
                        NormalValue = "1",
                        MeasurementUnit = "TestMeasurementUnit",
                        PatientConditionId = "TestPatientConditionId",
                        ConditionId = 1,
                        IsReviewed = 1
                 }
            };

            IEnumerable<GetPatientTargets> mappedpatientTargets = new List<GetPatientTargets>()
            {
                  new()
                  {
                       TargetId = 1,
                       TargetName = "TestTargetName",
                       MinValue = "TestMinValue",
                       MaxValue = "TestMaxValue",
                       BaseValue = "1",
                       TargetValue = "1",
                       RecentValue = "1",
                       NormalValue = "1",
                       MeasurementUnit = "TestMeasurementUnit",
                       PatientConditionId = "TestPatientConditionId",
                       ConditionId = 1,
                       IsReviewed = 1
                  }
            };

            List<GetPatientTargets> listOfPatientTargets = new List<GetPatientTargets>();

            _repositoryMock
              .Setup(repo => repo.GetPatientTargetsByPatientPlanIdAsync(1))
              .Returns(Task.FromResult(patientTargets));

            _mapperMock
            .Setup(mapper => mapper.Map<IEnumerable<Contracts.CarePlan.GetPatientTargets>>(patientTargets))
            .Returns(mappedpatientTargets);

            var serviceMock = new PatientTargetsService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            var results = await serviceMock.GetPatientTargetsByPatientPlanIdAsync(1);

            //Assert
            _repositoryMock.Verify(x => x.GetPatientTargetsByPatientPlanIdAsync(1), Times.Once());

            results.Should().NotBeNull();
            results.Should().BeOfType<List<Contracts.CarePlan.GetPatientTargets>>();
        }

        [Test]
        public async Task PostPatientTargetsAsyncSuccessTests()
        {
            //Arrange
            var mappedPatientTarget = BuildPatientPlan(_patientTarget);

            _repositoryMock
              .Setup(repo => repo.PostPatientTargetsAsync(mappedPatientTarget))
              .Returns(Task.FromResult(_patientTargetId));

            _mapperMock
            .Setup(mapper => mapper.Map<Models.PatientTargets>(_patientTarget))
            .Returns(mappedPatientTarget);

            var serviceMock = new PatientTargetsService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            var results = await serviceMock.PostPatientTargetsAsync(_patientTarget);

            //Assert
            _repositoryMock.Verify(x => x.PostPatientTargetsAsync(mappedPatientTarget), Times.Once());

            results.Should().Be(1);
        }

        [Test]
        public async Task DeletePatientTargetAsyncSuccessTests()
        {
            //Arrange
            _repositoryMock
              .Setup(repo => repo.DeletePatientTargetAsync(_patientTargetId))
              .Returns(Task.FromResult(1));

            var serviceMock = new PatientTargetsService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            var results = await serviceMock.DeletePatientTargetAsync(_patientTargetId);

            //Assert
            _repositoryMock.Verify(x => x.DeletePatientTargetAsync(_patientTargetId), Times.Once());

            results.Should().Be(1);
        }

        [Test]
        public async Task UpdatePatientTargetReviewStatusAsyncSuccessTests()
        {
            //Arrange
            var mappedPatientTarget = BuildUpdatePatientTargetReviewStatus(_updatePatientTargetReviewStatus);

            _mapperMock
           .Setup(mapper => mapper.Map<Models.UpdatePatientTargetReviewStatus>(_updatePatientTargetReviewStatus))
           .Returns(mappedPatientTarget);

            _repositoryMock
              .Setup(repo => repo.UpdatePatientTargetReviewStatusAsync(mappedPatientTarget, _patientTargetId))
              .Returns(Task.FromResult<long>(1));

            var serviceMock = new PatientTargetsService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            var results = await serviceMock.UpdatePatientTargetReviewStatusAsync(_updatePatientTargetReviewStatus, _patientTargetId);

            //Assert
            _repositoryMock.Verify(x => x.UpdatePatientTargetReviewStatusAsync(mappedPatientTarget, _patientTargetId), Times.Once());

            results.Should().Be(1);
        }

        [Test]
        public async Task UpdatePatientTargetsAsyncSuccessTests()
        {
            //Arrange
            var mappedPatientTarget = BuildUpdatePatientTargetStatus(_updatePatientTargets);

            _mapperMock
           .Setup(mapper => mapper.Map<Models.UpdatePatientTargets>(_updatePatientTargets))
           .Returns(mappedPatientTarget);

            _repositoryMock
              .Setup(repo => repo.UpdatePatientTargetsAsync(mappedPatientTarget, _patientTargetId))
              .Returns(Task.FromResult<long>(1));

            var serviceMock = new PatientTargetsService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            var results = await serviceMock.UpdatePatientTargetsAsync(_updatePatientTargets, _patientTargetId);

            //Assert
            _repositoryMock.Verify(x => x.UpdatePatientTargetsAsync(mappedPatientTarget, _patientTargetId), Times.Once());

            results.Should().Be(1);
        }

        private Models.PatientTargets BuildPatientPlan(PatientTargets request)
        {
            Models.PatientTargets patientTargets = new Models.PatientTargets()
            {
                ConditionId = request.ConditionId,
                TargetId = request.TargetId,
                MinValue = request.MinValue,
                MaxValue = request.MaxValue,
                MeasurementUnit = request.MeasurementUnit,
                CreatedBy = "TestCreatedBy",
                UpdatedBy = "TestUpdatedBy",
                TargetValue = request.TargetValue,
                BaseValue = request.BaseValue,
                RecentValue = request.RecentValue
            };

            return patientTargets;
        }

        private Models.UpdatePatientTargetReviewStatus BuildUpdatePatientTargetReviewStatus(UpdatePatientTargetReviewStatus request)
        {
            Models.UpdatePatientTargetReviewStatus updatePatientTargetReviewStatus = new Models.UpdatePatientTargetReviewStatus()
            {
                IsReviewed = request.IsReviewed
            };

            return updatePatientTargetReviewStatus;
        }

        private Models.UpdatePatientTargets BuildUpdatePatientTargetStatus(UpdatePatientTargets request)
        {
            Models.UpdatePatientTargets _updatePatientTargets = new Models.UpdatePatientTargets()
            {
                TargetId = request.TargetId,
                MinValue = request.MinValue,
                MaxValue = request.MaxValue,
                MeasurementUnit = request.MeasurementUnit,
                TargetValue = request.TargetValue,
                BaseValue = request.BaseValue,
                RecentValue = request.RecentValue
            };

            return _updatePatientTargets;
        }
    }
}