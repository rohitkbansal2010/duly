// <copyright file="PatientPlanServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan;
using Duly.CollaborationView.Encounter.Api.Services.Implementations.CarePlan;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations.CarePlan
{
    [TestFixture]
    public class PatientPlanServiceTests
    {
        private Mock<IPatientPlanRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private int _id = 1;
        private string _sId = "TestId";
        private PatientPlan _patientPlan = new PatientPlan()
        {
            AppointmentId = "TestAppointmentId",
            FlourishingStatement = "TestFlourishingStatement",
            PatientId = "TestPatientId",
            ProviderId = "TestProviderId"
        };

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IPatientPlanRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task PostPatientPlanAsyncSuccessTests()
        {
            //Arrange
            var mappedPatientPlan = BuildPatientPlan(_patientPlan);

            _repositoryMock
              .Setup(repo => repo.PostPatientPlanAsync(mappedPatientPlan))
              .Returns(Task.FromResult(1));

            _mapperMock
            .Setup(mapper => mapper.Map<Api.Repositories.Models.CarePlan.PatientPlan>(_patientPlan))
            .Returns(mappedPatientPlan);

            var serviceMock = new PatientPlanService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            var results = await serviceMock.PostPatientPlanAsync(_patientPlan);

            //Assert
            _repositoryMock.Verify(x => x.PostPatientPlanAsync(mappedPatientPlan), Times.Once());

            results.Should().Be(1);
        }

        [Test]
        public async Task UpdatePatientPlanStatusByIdAsyncSuccessTest()
        {
            _repositoryMock
             .Setup(repo => repo.UpdatePatientPlanStatusByIdAsync(_id))
             .Returns(Task.FromResult(true));

            var serviceMock = new PatientPlanService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            var results = await serviceMock.UpdatePatientPlanStatusByIdAsync(_id);

            //Assert
            _repositoryMock.Verify(x => x.UpdatePatientPlanStatusByIdAsync(_id), Times.Once());

            results.Should().Be(true);
        }

        [Test]
        public async Task GetPatientPlanByPatientIdAsyncSuccessTest()
        {
            //Arrange
            var getPatientPlanByPatientIdResponse = new Api.Repositories.Models.CarePlan.GetPatientPlanByPatientIdModel()
            {
                FlourishStatement = "TestFlourishingStatement",
                IsCompleted = true,
                PatientPlanId = 99,
                PlanName = "PlanName"
            };

            var mappedGetPatientPlanByPatientId = BuildGetPatientPlanByPatientIdResponse(getPatientPlanByPatientIdResponse);

            _repositoryMock
              .Setup(repo => repo.GetPatientPlanIdByPatientIdAsync(_sId))
              .Returns(Task.FromResult(getPatientPlanByPatientIdResponse));

            _mapperMock
            .Setup(mapper => mapper.Map<GetPatientPlanByPatientId>(getPatientPlanByPatientIdResponse))
            .Returns(mappedGetPatientPlanByPatientId);

            var serviceMock = new PatientPlanService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            var results = await serviceMock.GetPatientPlanByPatientIdAsync(_sId);

            //Assert
            _repositoryMock.Verify(x => x.GetPatientPlanIdByPatientIdAsync(_sId), Times.Once());

            results.Should().NotBeNull();
            results.Should().BeOfType<GetPatientPlanByPatientId>();
        }

        [Test]
        public async Task UpdateFlourishStatementAsyncSuccessTest()
        {
            var request = new UpdateFlourishStatementRequest()
            {
                FlourishStatement = "TestFlourishStatement",
                PatientPlanId = 99
            };

            _repositoryMock
             .Setup(repo => repo.UpdateFlourishStatementAsync(request))
             .Returns(Task.FromResult<long>(1));

            var serviceMock = new PatientPlanService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            var results = await serviceMock.UpdateFlourishStatementAsync(request);

            //Assert
            _repositoryMock.Verify(x => x.UpdateFlourishStatementAsync(request), Times.Once());

            results.Should().Be(1);
        }

        private Api.Repositories.Models.CarePlan.PatientPlan BuildPatientPlan(PatientPlan request)
        {
            Api.Repositories.Models.CarePlan.PatientPlan patientPlan = new Api.Repositories.Models.CarePlan.PatientPlan()
            {
                AppointmentId = request.AppointmentId,
                FlourishingStatement = request.FlourishingStatement,
                PatientId = request.PatientId,
                ProviderId = request.ProviderId
            };

            return patientPlan;
        }

        private GetPatientPlanByPatientId BuildGetPatientPlanByPatientIdResponse(Api.Repositories.Models.CarePlan.GetPatientPlanByPatientIdModel response)
        {
            var mappedResponse = new GetPatientPlanByPatientId()
            {
                PlanName = response.PlanName,
                PatientPlanId = response.PatientPlanId,
                FlourishStatement = response.FlourishStatement,
                IsCompleted = response.IsCompleted
            };

            return mappedResponse;
        }
    }
}