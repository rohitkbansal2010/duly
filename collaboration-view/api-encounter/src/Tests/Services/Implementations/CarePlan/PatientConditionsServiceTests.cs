// <copyright file="PatientConditionsServiceTests.cs" company="Duly Health and Care">
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
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations.CarePlan
{
    [TestFixture]
    public class PatientConditionsServiceTests
    {
        private Mock<IPatientConditionsRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;

        private PatientConditions _patientConditions = new PatientConditions()
        {
            AddConditionIds = new long[] { 1, 3 },
            PatientPlanId = 1,
            RemoveConditionIds = new long[] { 1, 4 }
        };

        private IEnumerable<Models.GetPatientConditionByPatientPlanIdModel> _getPatientConditionByPatientPlanIdModelResponse = new List<Models.GetPatientConditionByPatientPlanIdModel>()
        {
           new Models.GetPatientConditionByPatientPlanIdModel()
           {
            ConditionDisplayName = "TestConditionDisplayName",
            ConditionId = 1,
            ConditionShortName = "TestConditionShortName",
            PatientConditionId = 1
           }
        };

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IPatientConditionsRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task PostPatientConditionsAsyncSuccessTest()
        {
            //Arrange
            var mappedRequestPatientConditions = BuildPostPatientConditionsRequest(_patientConditions);
            IEnumerable<long> response = new List<long>() { 1, 2 };

            _mapperMock
            .Setup(mapper => mapper.Map<Models.PatientConditions>(_patientConditions))
            .Returns(mappedRequestPatientConditions);

            _repositoryMock
              .Setup(repo => repo.PostPatientConditionsAsync(mappedRequestPatientConditions, null))
              .Returns(Task.FromResult(response));

            _mapperMock
            .Setup(mapper => mapper.Map<IEnumerable<long>>(response))
            .Returns(response);

            var serviceMock = new PatientConditionsService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            var results = await serviceMock.PostPatientConditionsAsync(_patientConditions);

            //Assert
            _repositoryMock.Verify(x => x.PostPatientConditionsAsync(mappedRequestPatientConditions, null), Times.Once());

            results.Should().NotBeNull();
            results.Should().BeOfType<List<long>>();
        }

        [Test]
        public async Task GetConditionByPatientPlanId()
        {
            //Arrange
            long id = 1;
            var mappedResponse = BuildGetConditionByPatientPlanIdResponse(_getPatientConditionByPatientPlanIdModelResponse);

            _repositoryMock
              .Setup(repo => repo.GetConditionByPatientPlanId(id))
              .Returns(Task.FromResult(_getPatientConditionByPatientPlanIdModelResponse));

            _mapperMock
            .Setup(mapper => mapper.Map<List<GetPatientConditionByPatientPlanId>>(_getPatientConditionByPatientPlanIdModelResponse))
            .Returns(mappedResponse);

            var serviceMock = new PatientConditionsService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            var results = await serviceMock.GetConditionByPatientPlanId(id);

            //Assert
            _repositoryMock.Verify(x => x.GetConditionByPatientPlanId(id), Times.Once());

            results.Should().NotBeNull();
            results.Should().BeOfType<List<GetPatientConditionByPatientPlanId>>();
        }

        private Models.PatientConditions BuildPostPatientConditionsRequest(PatientConditions request)
        {
            Models.PatientConditions patientConditions = new Models.PatientConditions()
            {
                AddConditionIds = request.AddConditionIds,
                PatientPlanId = request.PatientPlanId,
                RemoveConditionIds = request.RemoveConditionIds
            };

            return patientConditions;
        }

        private List<GetPatientConditionByPatientPlanId> BuildGetConditionByPatientPlanIdResponse(IEnumerable<Models.GetPatientConditionByPatientPlanIdModel> request)
        {
            var req = request.ToArray();
            List<GetPatientConditionByPatientPlanId> response = new List<GetPatientConditionByPatientPlanId>()
            {
                new GetPatientConditionByPatientPlanId()
                {
                 ConditionDisplayName = req[0].ConditionDisplayName,
                 ConditionId = req[0].ConditionId,
                 ConditionShortName = req[0].ConditionShortName,
                 PatientConditionId = req[0].PatientConditionId
                }
            };

            return response;
        }
    }
}