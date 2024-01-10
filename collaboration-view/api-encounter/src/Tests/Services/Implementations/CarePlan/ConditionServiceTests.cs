// <copyright file="ConditionServiceTests.cs" company="Duly Health and Care">
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
    public class ConditionServiceTests
    {
        private Mock<IConditionRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IConditionRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task GetConditionsSuccessTests()
        {
            //Arrange
            IEnumerable<Api.Repositories.Models.CarePlan.Condition> conditions = new Api.Repositories.Models.CarePlan.Condition[]
            {
                 new()
                 {
                     ConditionId = 1,
                     ConditionDisplayName = "TestConditionDisplayName",
                     ConditionShortName = "TestConditionShortName",
                     Active = true,
                 }
            };

            IEnumerable<Contracts.CarePlan.Condition> mappedConditions = new Contracts.CarePlan.Condition[]
            {
                  new()
                  {
                      ConditionId = 1,
                      ConditionDisplayName = "TestConditionDisplayName",
                      ConditionShortName = "TestConditionShortName",
                      Active = true,
                  }
            };

            List<GetConditionResponse> listOfConditions = new List<GetConditionResponse>();

            _repositoryMock
              .Setup(repo => repo.GetConditions())
              .Returns(Task.FromResult(conditions));

            _mapperMock
            .Setup(mapper => mapper.Map<IEnumerable<Contracts.CarePlan.Condition>>(conditions))
            .Returns(mappedConditions);

            var serviceMock = new ConditionService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            var results = await serviceMock.GetConditions();

            //Assert
            _repositoryMock.Verify(x => x.GetConditions(), Times.Once());

            results.Should().NotBeNull();
            results.Should().BeOfType<List<GetConditionResponse>>();
        }
    }
}