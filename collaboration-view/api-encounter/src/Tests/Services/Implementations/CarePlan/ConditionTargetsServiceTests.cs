// <copyright file="ConditionTargetsServiceTests.cs" company="Duly Health and Care">
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
namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations.CarePlan
{
    [TestFixture]
    public class ConditionTargetsServiceTests
    {
        private Mock<IConditionTargetsRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IConditionTargetsRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task GetConditionsSuccessTests()
        {
            //Arrange
            IEnumerable<Api.Repositories.Models.CarePlan.ConditionTargets> conditionsTargets = new Api.Repositories.Models.CarePlan.ConditionTargets[]
            {
                 new()
                 {
                 }
            };

            IEnumerable<Contracts.CarePlan.GetConditionTargetsResponse> mappedConditionsTargets = new Contracts.CarePlan.GetConditionTargetsResponse[]
            {
                  new()
                  {
                  }
            };

            List<GetConditionTargetsResponse> listOfConditions = new List<GetConditionTargetsResponse>();

            _repositoryMock
              .Setup(repo => repo.GetConditionTargetsByConditionId("text"))
              .Returns(Task.FromResult(conditionsTargets));

            _mapperMock
            .Setup(mapper => mapper.Map<IEnumerable<Contracts.CarePlan.GetConditionTargetsResponse>>(conditionsTargets))
            .Returns(mappedConditionsTargets);

            var serviceMock = new ConditionTargetsService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            //var results = await serviceMock.GetConditions();
            var results = await serviceMock.GetConditionTargetsByConditionId("text");

            //Assert
            _repositoryMock.Verify(x => x.GetConditionTargetsByConditionId("text"), Times.Once());

            results.Should().NotBeNull();
            results.Should().BeOfType<List<GetConditionTargetsResponse>>();
        }
    }
}
